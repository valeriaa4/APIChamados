namespace APIChamados.Models
{
    public class HistoricoInteracoes
    {
        public int IdInteracao { get; set; }
        public int IdChamado { get; set; }
        public int IdTecnico { get; set; }
        public DateTime DataInteracao { get; set; }
        public string Resposta { get; set; }

        public HistoricoInteracoes(int idInteracao, int idChamado, int idTecnico, DateTime dataInteracao, string resposta)
        {
            this.IdInteracao = idInteracao;
            this.IdChamado = idChamado;
            this.IdTecnico = idTecnico;
            this.DataInteracao = dataInteracao;
            this.Resposta = resposta;
        }
    }
}
