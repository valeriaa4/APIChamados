using APIChamados.Dtos;
using APIChamados.Models;
using APIChamados.Repositories;

namespace APIChamados.Services
{
    public class InteracaoService
    {
        public readonly IInteracaoRepository _interacaoRepository;

        public InteracaoService(IInteracaoRepository interacaoRepository)
        {
            _interacaoRepository = interacaoRepository;
        }

        public async Task<IEnumerable<Interacao>> GetAllInteracoesAsync()
        {
            return await _interacaoRepository.GetAllAsync();
        }

        public async Task<Interacao?> GetInteracaoByIdAsync(int id)
        {
            return await _interacaoRepository.GetByIdAsync(id);
        }

        public async Task<Interacao> AddInteracaoAsync(InteracaoDto interacaoDto)
        {
            return await _interacaoRepository.AddAsync(interacaoDto);
        }

        public async Task UpdateRespostaAsync(int id, string resposta)
        {
            await _interacaoRepository.UpdateRespostaAsync(id, resposta);
        }

        public async Task DeleteInteracaoAsync(int id)
        {
            await _interacaoRepository.DeleteAsync(id);
        }
    }
}
