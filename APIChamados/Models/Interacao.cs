using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace APIChamados.Models
{
    public class Interacao
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdInteracao { get; set; }
        [Required]
        public DateTime DataInteracao { get; set; }
        [Required]
        public string Resposta { get; set; }

        [Required]
        [ForeignKey("idChamado")]
        public int IdChamado { get; set; }
        [JsonIgnore]
        public Chamado Chamado { get; set; }

        public Interacao() { }

        public Interacao (int idChamado, DateTime dataInteracao, string resposta)
        {
            this.IdChamado = idChamado;
            this.DataInteracao = dataInteracao;
            this.Resposta = resposta;
        }
    }
}
