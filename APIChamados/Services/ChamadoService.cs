using APIChamados.Dtos;
using APIChamados.Models;
using APIChamados.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Collections;

namespace APIChamados.Services
{
    public class ChamadoService
    {
        private readonly IChamadoRepository _chamadoRepository;

        public ChamadoService(IChamadoRepository chamadoRepository)
        {
            _chamadoRepository = chamadoRepository;
        }

        public async Task<IEnumerable> GetAllChamadosAsync()
        {
            return await _chamadoRepository.GetAllAsync();
        }

        public async Task<Chamado?> GetChamadoByProtocoloAsync(int protocolo)
        {
            return await _chamadoRepository.GetByProtocoloAsync(protocolo);
        }

        public async Task<Chamado?> GetChamadoByIdAsync(int id)
        {
            return await _chamadoRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Chamado?>> GetChamadosByPrioridadeAsync(int prioridade)
        {
            return await _chamadoRepository.GetByPrioridadeAsync(prioridade);
        }

        public async Task<IEnumerable<Chamado?>> GetChamadosByStatusAsync(int status)
        {
            return await _chamadoRepository.GetByStatusAsync(status);
        }

        public async Task<Chamado> AddChamadoAsync(ChamadoDto chamadoDto)
        {
            return await _chamadoRepository.AddAsync(chamadoDto);
        }

        public async Task UpdateChamadoStatusAsync(int id, int status)
        {
            await _chamadoRepository.UpdateStatusAsync(id, status);
        }

        public async Task UpdateChamadoPrioridadeAsync(int id, int prioridade)
        {
            await _chamadoRepository.UpdatePrioridadeAsync(id, prioridade);
        }

        public async Task UpdateChamadoTituloAsync(int id, string titulo)
        {
            await _chamadoRepository.UpdateTituloAsync(id, titulo);
        }

        public async Task UpdateChamadoDescricaoAsync(int id, string descricao)
        {
            await _chamadoRepository.UpdateDescricaoAsync(id, descricao);
        }

        public async Task DeleteChamadoAsync(int id)
        {
            await _chamadoRepository.DeleteAsync(id);
        }
    }
}
