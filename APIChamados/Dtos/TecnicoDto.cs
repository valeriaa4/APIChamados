namespace APIChamados.Dtos
{
    public class TecnicoDto
    {
        public string Nome { get; set; }
        public string Telefone { get; set; }
        public string Email { get; set; }
        public string Senha { get; set; }
        public DateTime DataContratacao { get; set; }
        public bool Administrador { get; set; }
    }
}
