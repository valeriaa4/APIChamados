using APIChamados.Dtos;
using APIChamados.Models;

namespace APIChamados.Repositories
{
    public interface IInteracaoRepository
    {
        Task<IEnumerable<Interacao>> GetAllAsync();
        Task<Interacao?> GetByIdAsync(int id);
        Task<Interacao> AddAsync(InteracaoDto interacaoDto);
        Task UpdateRespostaAsync(int id, string resposta);
        Task DeleteAsync(int id);
    }
}
