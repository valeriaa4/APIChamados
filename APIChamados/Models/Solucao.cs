namespace APIChamados.Models
{
    public class Solucao
    {
        public int IdSolucao { get; set; }
        public int IdChamado { get; set; }
        public string Descricao { get; set; }
        public DateTime DataSolucao { get; set; }

        public Solucao(int idSolucao, int idChamado, string descricao, DateTime dataSolucao)
        {
            this.IdSolucao = idSolucao;
            this.IdChamado = idChamado;
            this.Descricao = descricao;
            this.DataSolucao = dataSolucao;
        }
    }
}
