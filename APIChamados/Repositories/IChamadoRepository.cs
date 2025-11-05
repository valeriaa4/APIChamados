using APIChamados.Models;
using APIChamados.Dtos;

namespace APIChamados.Repositories
{
    public interface IChamadoRepository
    {
        Task<IEnumerable<Chamado>> GetAllAsync();
        Task<Chamado?> GetByIdAsync(int id);
        Task<Chamado?> GetByProtocoloAsync(int protocolo);
        Task<IEnumerable<Chamado?>> GetByPrioridadeAsync(int prioridade);
        Task<IEnumerable<Chamado?>> GetByStatusAsync(int status);
        Task<Chamado> AddAsync(ChamadoDto chamadoDto);
        Task UpdateStatusAsync(int id, int status);
        Task UpdatePrioridadeAsync(int id, int prioridade);
        Task UpdateTituloAsync(int id, string titulo);
        Task UpdateDescricaoAsync(int id, string descricao);
        Task DeleteAsync(int id);
    }
}
