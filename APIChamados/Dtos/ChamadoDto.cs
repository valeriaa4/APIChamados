using APIChamados.Enums;

namespace APIChamados.Dtos
{
    public class ChamadoDto
    {
        public int IdUsuario { get; set; }
        public int IdTecnico { get; set; }
        public string Titulo { get; set; }
        public string Descricao { get; set; }
        public Status Status { get; set; }
        public Prioridade Prioridade { get; set; }
    }
}
