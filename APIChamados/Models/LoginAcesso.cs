namespace APIChamados.Models
{
    public class LoginAcesso
    {
        public int IdLogAcesso { get; set; }
        public int IdUsuario { get; set; } // atraves do id, pegar email
        public DateTime DataHoraAcesso { get; set; }

        public LoginAcesso(int idLogAcesso, int idUsuario, DateTime dataHoraAcesso)
        {
            this.IdLogAcesso = idLogAcesso;
            this.IdUsuario = idUsuario;
            this.DataHoraAcesso = dataHoraAcesso;
        }
    }
}