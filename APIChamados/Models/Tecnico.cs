using System.ComponentModel.DataAnnotations;

namespace APIChamados.Models
{
    public class Tecnico : Usuario
    {
        [Required]
        public string Senha { get; set; }
        [Required]
        public bool Administrador { get; set; }
        [Required]
        public DateTime DataContratacao { get; set; }
        public Tecnico() { }

        public Tecnico(string nome, string email, string telefone, string senha, bool administrador, DateTime dataContratacao) : base(nome, email, telefone)
        {
            this.Senha = senha;
            this.Administrador = administrador;
            this.DataContratacao = dataContratacao;
        }
    }
}
