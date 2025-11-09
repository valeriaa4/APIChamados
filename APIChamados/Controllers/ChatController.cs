using APIChamados.Enums;
using APIChamados.Services;
using Microsoft.AspNetCore.Mvc;

namespace APIChamados.Controllers
{
    [ApiController]
    [Route("api/chat")]
    public class ChatController : ControllerBase
    {

        private readonly ChatAiHttpService _chat;
        public ChatController(ChatAiHttpService chat) => _chat = chat;

        // DTO usado para desserializar o JSON de entrada: { "message": "..." }
        public class ChatRequest { public string? Message { get; set; } }

        [HttpPost]
        public async Task<IActionResult> Send([FromBody] ChatRequest request, CancellationToken ct)
        {
            // Validação simples: evita chamadas vazias.
            if (string.IsNullOrWhiteSpace(request.Message))
                return BadRequest("Mensagem vazia.");

            var reply = await _chat.PerguntarAsync(request.Message!, ct);

            return Ok(new{ reply });
        }

        [HttpPost("/chamado")]
        public async Task<(string titulo, Prioridade prioridade)> DefinirPrioridadeETituloAsync([FromBody] string descricao)
        {
            return await _chat.GerarTituloEPrioridadeAsync(descricao);
        }
    }
}
