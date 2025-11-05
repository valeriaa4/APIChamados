using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APIChamados.Models
{
    public class Usuario
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string Nome { get; set; }
        [Required, EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Telefone { get; set; }

        public Usuario() { }
        public Usuario(string nome, string email, string telefone)
        {
            this.Nome = nome;
            this.Email = email;
            this.Telefone = telefone;
        }
    }
}
