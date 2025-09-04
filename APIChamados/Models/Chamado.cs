using APIChamados.Enums;

namespace APIChamados.Models
{
    public class Chamado
    {
        public int IdChamado { get; set; }
        public int IdUsuario { get; set; }
        public int IdCategoria { get; set; }
        public int IdTecnico { get; set; }
        public string Titulo { get; set; }
        public string Descricao { get; set; }
        public DateTime DataAbertura { get; set; }
        public DateTime? DataConclusao { get; set; }
        public Status Status { get; set; }
        public Prioridade Prioridade { get; set; }
        public List<HistoricoInteracoes>? HistoricoInteracoes { get; set; }
        public Solucao? Solucao { get; set; }

        public Chamado(int idChamado, int idUsuario, int idCategoria, int idTecnico, string titulo, string descricao, DateTime dataAbertura, DateTime? dataConclusao, Status status, Prioridade prioridade, Solucao solucao)
        {
            this.IdChamado = idChamado;
            this.IdUsuario = idUsuario;
            this.IdCategoria = idCategoria;
            this.IdTecnico = idTecnico;
            this.Titulo = titulo;
            this.Descricao = descricao;
            this.DataAbertura = dataAbertura;
            this.DataConclusao = dataConclusao;
            this.Status = status;
            this.Prioridade = prioridade;
            this.HistoricoInteracoes = new List<HistoricoInteracoes>();
            this.Solucao = solucao;
        }
    }
}
