using APIChamados.Enums;
using System.Net;
using System.Text.Json;

namespace APIChamados.Services
{
    public class ChatAiHttpService
    {
        private readonly IHttpClientFactory _factory;
        private readonly ILogger<ChatAiHttpService> _logger;

        public ChatAiHttpService(IHttpClientFactory factory, ILogger<ChatAiHttpService> logger)
        {
            _factory = factory;
            _logger = logger;
        }

        public async Task<string> PerguntarAsync(string pergunta, CancellationToken ct = default)
        {
            var payload = new
            {
                model = "gpt-4o-mini",
                messages = new[]
                {
                    new { role = "system", content = @"Você é um assistente virtual que presta suporte técnico a funcionários de uma empresa. 
                                                       Responda de modo claro e objetivo" },
                    new { role = "user", content = pergunta }
                },
                temperature = 0.3
            };

            var http = _factory.CreateClient("OpenAI");

            const int maxRetries = 3;
            var rnd = new Random();

            for (int attempt = 1; attempt <= maxRetries; attempt++)
            {
                using var resp = await http.PostAsJsonAsync("chat/completions", payload, ct);
                var body = await resp.Content.ReadAsStringAsync(ct);

                if (resp.IsSuccessStatusCode)
                {
                    using var json = JsonDocument.Parse(body);
                    var content = json.RootElement
                        .GetProperty("choices")[0]
                        .GetProperty("message")
                        .GetProperty("content")
                        .GetString();

                    return string.IsNullOrWhiteSpace(content)
                        ? "Não consegui gerar uma resposta agora."
                        : content;
                }

                if (resp.StatusCode == (HttpStatusCode)429 || (int)resp.StatusCode >= 500)
                {
                    TimeSpan delay;
                    if (resp.Headers.TryGetValues("Retry-After", out var vals) &&
                        int.TryParse(vals.FirstOrDefault(), out var seconds) && seconds >= 0)
                    {
                        delay = TimeSpan.FromSeconds(seconds);
                    }
                    else
                    {
                        var baseDelay = TimeSpan.FromSeconds(Math.Pow(2, attempt - 1));
                        var jitter = TimeSpan.FromMilliseconds(rnd.Next(0, 250));
                        delay = baseDelay + jitter;
                    }

                    _logger.LogWarning(
                        "OpenAI {Status}. Retentando em {Delay}s (tentativa {Attempt}/{Max}). Corpo: {Body}",
                        (int)resp.StatusCode, delay.TotalSeconds, attempt, maxRetries, body
                    );

                    if (attempt == maxRetries)
                    {
                        var msg = ExtrairMensagemErro(body) ?? "Limite de requisições atingido. Tente novamente em instantes.";
                        return $"Falha (HTTP {(int)resp.StatusCode}): {msg}";
                    }

                    await Task.Delay(delay, ct);
                    continue;
                }

                _logger.LogError("OpenAI falhou: {Status} - {Body}", (int)resp.StatusCode, body);
                var erro = ExtrairMensagemErro(body) ?? body;
                return $"Falha (HTTP {(int)resp.StatusCode}): {erro}";
            }

            return "Não consegui gerar uma resposta agora.";
        }

        public async Task<(string titulo, Prioridade prioridade)> GerarTituloEPrioridadeAsync(string descricao, CancellationToken ct = default)
        {
            var payload = new
            {
                model = "gpt-4o-mini",
                messages = new[]
                {
                new { role = "system",
                      content = @"Você é um assistente técnico. Analise a descrição de um chamado e devolva um JSON simples com dois campos:
                                - titulo: um resumo genérico da solicitação (máximo 6 palavras)
                                - prioridade: uma das opções Baixa, Média ou Alta, conforme a urgência" },
                new { role = "user", content = $"Descrição: {descricao}" }
                },
                temperature = 0.3
            };

            var http = _factory.CreateClient("OpenAI");
            var resp = await http.PostAsJsonAsync("chat/completions", payload, ct);
            var body = await resp.Content.ReadAsStringAsync(ct);

            using var json = JsonDocument.Parse(body);
            var content = json.RootElement
                .GetProperty("choices")[0]
                .GetProperty("message")
                .GetProperty("content")
                .GetString();

            try
            {
                _logger.LogInformation("Resposta bruta da IA: {Resposta}", content);

                // Garante que só o JSON seja processado (caso venha texto extra)
                var jsonStart = content.IndexOf('{');
                var jsonEnd = content.LastIndexOf('}');
                if (jsonStart == -1 || jsonEnd == -1)
                    throw new Exception("Resposta da IA não contém JSON válido.");

                var jsonOnly = content.Substring(jsonStart, jsonEnd - jsonStart + 1);

                using var resultJson = JsonDocument.Parse(jsonOnly);
                var root = resultJson.RootElement;

                // Lê diretamente as propriedades esperadas
                var titulo = root.GetProperty("titulo").GetString() ?? "Chamado sem título";
                var prioridadeTexto = root.GetProperty("prioridade").GetString() ?? "Média";

                // Converte o texto para enum Prioridade (0=Baixa, 1=Média, 2=Alta)
                Prioridade prioridade = prioridadeTexto.ToLower() switch
                {
                    var t when t.Contains("alta") => Prioridade.Alta,
                    var t when t.Contains("méd") || t.Contains("med") => Prioridade.Media,
                    _ => Prioridade.Baixa
                };

                _logger.LogInformation("Título definido pela IA: {titulo}, Prioridade: {prioridade}", titulo, prioridade);

                return (titulo, prioridade);
            }
            catch (Exception ex)
            {
                _logger.LogWarning("Falha ao interpretar JSON da IA: {Mensagem}. Conteúdo: {Conteudo}", ex.Message, content);
                return ("Chamado genérico", Prioridade.Media);
            }
        }

        private static string? ExtrairMensagemErro(string body)
        {
            try
            {
                using var doc = JsonDocument.Parse(body);
                if (doc.RootElement.TryGetProperty("error", out var e))
                    return e.TryGetProperty("message", out var m) ? m.GetString() : e.ToString();
            }
            catch { }
            return null;
        }
    }
}