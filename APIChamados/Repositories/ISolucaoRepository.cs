using APIChamados.Dtos;
using APIChamados.Models;

namespace APIChamados.Repositories
{
    public interface ISolucaoRepository
    {
        public Task<IEnumerable<Solucao>> GetAllAsync();
        public Task<Solucao?> GetByIdAsync(int id);
        public Task<Solucao> AddAsync(SolucaoDto solucaoDto);
        public Task UpdateDescricaoAsync(int id, string descricao);
        public Task DeleteAsync(int id);
    }
}
