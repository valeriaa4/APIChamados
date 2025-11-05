using APIChamados.Models;
using APIChamados.Dtos;

namespace APIChamados.Repositories
{
    public interface ITecnicoRepository
    {
        Task<IEnumerable<Tecnico>> GetAllAsync();
        Task<Tecnico?> GetByIdAsync(int id); 
        Task<Tecnico> AddAsync(TecnicoDto tecnicoDto);
        Task<Tecnico> GetByEmailAndSenhaAsync(string email, string senha);
        Task UpdateAsync(Tecnico tecnico);
        Task DeleteAsync(int id);
    }
}
