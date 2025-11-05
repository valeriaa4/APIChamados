namespace APIChamados.Models
{
    public class Resposta
    {
        public int Id { get; set; }
        public string Conteudo { get; set; } = string.Empty;        public int RequisicaoId { get; set; }
        public Requisicao Requisicao { get; set; }
    }
}
