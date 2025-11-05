using APIChamados.Dtos;
using APIChamados.Enums;
using APIChamados.Models;
using Microsoft.EntityFrameworkCore;
using static APIChamados.Data.ApplicationDBContext;

namespace APIChamados.Repositories
{
    public class SolucaoRepository : ISolucaoRepository
    {
        public readonly AppDbContext _context;

        public SolucaoRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Solucao?> GetByIdAsync(int id)
        {
            return await _context.Solucoes.FindAsync(id);
        }

        public async Task<IEnumerable<Solucao>> GetAllAsync()
        {
            return await _context.Solucoes.ToListAsync();
        }

        public async Task<Solucao> AddAsync(SolucaoDto solucaoDto)
        {
            var solucao = new Solucao
            {
                IdChamado = solucaoDto.IdChamado,
                Descricao = solucaoDto.Descricao,
                DataSolucao = DateTime.Now
            };
            _context.Solucoes.Add(solucao);
            await _context.SaveChangesAsync();

            // altera o status do chamado para fechado
            var chamado = await _context.Chamados.FindAsync(solucaoDto.IdChamado);
            if (chamado != null)
            {
                chamado.Status = Status.Fechado;
                chamado.DataConclusao = DateTime.Now;
                await _context.SaveChangesAsync();
            }
            
            return solucao;
        }

        public async Task UpdateDescricaoAsync(int id, string descricao)
        {
            var solucao = await _context.Solucoes.FindAsync(id);
            if (solucao != null)
            {
                solucao.Descricao = descricao;
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteAsync(int id)
        {
            var solucao = await _context.Solucoes.FindAsync(id);
            if (solucao != null)
            {
                _context.Solucoes.Remove(solucao);
                await _context.SaveChangesAsync();
            }
        }
    }
}
