using APIChamados.Dtos;
using APIChamados.Enums;
using APIChamados.Models;
using Microsoft.EntityFrameworkCore;
using static APIChamados.Data.ApplicationDBContext;

namespace APIChamados.Repositories
{
    public class InteracaoRepository : IInteracaoRepository
    {
       public readonly AppDbContext _context;
        public InteracaoRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<Interacao> AddAsync(InteracaoDto interacaoDto)
        {
            var interacao = new Interacao
            {
                IdChamado = interacaoDto.IdChamado,
                Resposta = interacaoDto.Resposta,
                DataInteracao = DateTime.Now
            };
            _context.Interacoes.Add(interacao);
            await _context.SaveChangesAsync();

            // Adiciona a interação ao histórico do chamado
            var chamado = await _context.Chamados
                .Include(c => c.HistoricoInteracoes)
                .FirstOrDefaultAsync(c => c.IdChamado == interacaoDto.IdChamado);

            if (chamado != null)
            {
                chamado.HistoricoInteracoes.Add(interacao);
                chamado.Status = Status.EmAndamento;
                await _context.SaveChangesAsync();
            }

            return interacao;
        }

        public async Task<IEnumerable<Interacao>> GetAllAsync()
        {
            return await _context.Interacoes.ToListAsync();
        }

        public async Task<Interacao?> GetByIdAsync(int id)
        {
            return await _context.Interacoes.FindAsync(id);
        }

        public async Task UpdateRespostaAsync(int id, string resposta)
        {
            var interacao = await _context.Interacoes.FindAsync(id);
            if (interacao != null)
            {
                interacao.Resposta = resposta;
                await _context.SaveChangesAsync();
            }
        }
         
        public async Task DeleteAsync(int id)
        {
            var interacao = await _context.Interacoes.FindAsync(id);
            if (interacao != null)
            {
                _context.Interacoes.Remove(interacao);
                await _context.SaveChangesAsync();
            }
        }
    }
}
