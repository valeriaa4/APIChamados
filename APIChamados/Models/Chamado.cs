using APIChamados.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APIChamados.Models
{
    public class Chamado
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdChamado { get; set; }
        [Required]
        public int Protocolo { get; set; }
        [Required]
        public string Titulo { get; set; }
        [Required]
        public string Descricao { get; set; }
        [Required]
        public DateTime DataAbertura { get; set; }
        public DateTime? DataConclusao { get; set; }
        [Required]
        public Status Status { get; set; }
        [Required]
        public Prioridade Prioridade { get; set; }
        public List<Interacao> HistoricoInteracoes { get; set; } = new();
        public Solucao? Solucao { get; set; }
        
        [Required]
        [ForeignKey("idUsuario")]
        public int IdUsuario { get; set; }
        public Usuario Usuario { get; set; }

        [Required]
        [ForeignKey("idTecnico")]
        public int IdTecnico { get; set; }
        public Tecnico Tecnico { get; set; }

        public Chamado() { }
        public Chamado(int idUsuario, int idTecnico, int protocolo, string titulo, string descricao, Status status, Prioridade prioridade)
        {
            this.IdUsuario = idUsuario;
            this.IdTecnico = idTecnico;
            this.Protocolo = protocolo;
            this.Titulo = titulo;
            this.Descricao = descricao;
            this.Status = status;
            this.Prioridade = prioridade;
        }

        public Chamado(int idUsuario, int idTecnico, string titulo, string descricao, DateTime dataAbertura, DateTime? dataConclusao, Status status, Prioridade prioridade, Solucao solucao, Interacao interacao)
        {
            this.IdUsuario = idUsuario;
            this.IdTecnico = idTecnico;
            this.Titulo = titulo;
            this.Descricao = descricao;
            this.DataAbertura = dataAbertura;
            this.DataConclusao = dataConclusao;
            this.Status = status;
            this.Prioridade = prioridade;
            this.HistoricoInteracoes = new List<Interacao>();
            this.Solucao = solucao;
        }
    }
}
