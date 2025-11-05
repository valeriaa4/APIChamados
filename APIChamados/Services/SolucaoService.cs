using APIChamados.Dtos;
using APIChamados.Models;
using APIChamados.Repositories;
using System.Collections;

namespace APIChamados.Services
{
    public class SolucaoService
    {
        public readonly ISolucaoRepository _solucaoRepository;

        public SolucaoService(ISolucaoRepository solucaoRepository)
        {
            _solucaoRepository = solucaoRepository;
        }

        public async Task<IEnumerable> GetAllSolucoesAsync()
        {
            var solucoes = await _solucaoRepository.GetAllAsync();
            return solucoes;
        }

        public async Task<Solucao?> GetSolucaoByIdAsync(int id)
        {
            return await _solucaoRepository.GetByIdAsync(id);
        }

        public async Task<Solucao> AddSolucaoAsync(SolucaoDto solucaoDto)
        {
            return await _solucaoRepository.AddAsync(solucaoDto);
        }

        public async Task UpdateDescricaoAsync(int id, string descricao)
        {
            await _solucaoRepository.UpdateDescricaoAsync(id, descricao);
        }

        public async Task DeleteSolucaoAsync(int id)
        {
            await _solucaoRepository.DeleteAsync(id);
        }
    }
}
