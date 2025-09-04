namespace APIChamados.Models
{
    public class Tecnico : Usuario
    {
        public string Senha { get; set; }
        public bool Administrador { get; set; }
        public DateTime DataContratacao { get; set; }
        public List<Chamado>? ChamadosAbertos { get; set; }
        public List<Chamado>? ChamadosFinalizados { get; set; }

        public Tecnico(int id, string nome, string email, int telefone, string senha, bool administrador, DateTime dataContratacao) : base(id, nome, email, telefone)
        {
            this.Senha = senha;
            this.Administrador = administrador;
            this.DataContratacao = dataContratacao;
            this.ChamadosAbertos = new List<Chamado>();
            this.ChamadosFinalizados = new List<Chamado>();
        }
    }
}
