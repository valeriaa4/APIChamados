using APIChamados.Dtos;
using APIChamados.Enums;
using APIChamados.Models;
using APIChamados.Services;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using static APIChamados.Data.ApplicationDBContext;

namespace APIChamados.Repositories
{
    public class ChamadoRepository : IChamadoRepository
    {
        private readonly AppDbContext _context;
        private readonly ChatAiHttpService _chat;

        public ChamadoRepository(AppDbContext context, ChatAiHttpService chat)
        {
            _context = context;
            _chat = chat;
        }

        public async Task<IEnumerable<Chamado>> GetAllAsync()
        {
            return await _context.Chamados
                .Include(c => c.Usuario)
                .Include(c => c.Tecnico)
                .Include(c => c.Solucao)
                .Include(c => c.HistoricoInteracoes)
                .ToListAsync();
        }

        public async Task<Chamado?> GetByIdAsync(int id)
        {
            return await _context.Chamados
                .Include(c => c.Usuario)
                .Include(c => c.Tecnico)
                .Include(c => c.Solucao)
                .Include(c => c.HistoricoInteracoes)
                .FirstOrDefaultAsync(c => c.IdChamado == id);
        }

        public async Task<IEnumerable<Chamado>> GetByTecnicoAsync(int id)
        {
            return await _context.Chamados
                .Include(c => c.Usuario)
                .Include(c => c.Tecnico)
                .Include(c => c.Solucao)
                .Include(c => c.HistoricoInteracoes)
                .Where(c => c.IdTecnico == id)
                .ToListAsync();
        }

        public async Task<Chamado?> GetByProtocoloAsync(int protocolo)
        {
            return await _context.Chamados
                .Include(c => c.Usuario)
                .Include(c => c.Tecnico)
                .Include(c => c.Solucao)
                .Include(c => c.HistoricoInteracoes)
                .FirstOrDefaultAsync(c => c.Protocolo == protocolo);
        }

        public async Task<IEnumerable<Chamado?>> GetByPrioridadeAsync(int prioridade)
        {
            return await _context.Chamados
                .Include(c => c.Usuario)
                .Include(c => c.Tecnico)
                .Include(c => c.Solucao)
                .Include(c => c.HistoricoInteracoes)
                .Where(c => (int)c.Prioridade == prioridade)
                .ToListAsync();
        }

        public async Task<IEnumerable<Chamado?>> GetByStatusAsync(int status)
        {
            return await _context.Chamados
                .Include(c => c.Usuario)
                .Include(c => c.Tecnico)
                .Include(c => c.Solucao)
                .Include(c => c.HistoricoInteracoes)
                .Where(c => (int)c.Status == status)
                .ToListAsync();
        }

        public async Task<Chamado> AddAsync(ChamadoDto chamadoDto)
        {

            var newProtocolo = await GerarProtocoloAsync();
            var tecnico = await SelecionarTecnicoAsync();
            var (titulo, prioridade) = await _chat.GerarTituloEPrioridadeAsync(chamadoDto.Descricao);
            var chamado = new Chamado
            {
                IdUsuario = chamadoDto.IdUsuario,
                IdTecnico = tecnico,
                Protocolo = newProtocolo,
                Titulo = titulo,
                Descricao = chamadoDto.Descricao,
                DataAbertura = DateTime.Now,
                Status = 0,
                Prioridade = prioridade
            };
            _context.Chamados.Add(chamado);
            await _context.SaveChangesAsync();
            return chamado;
        }

        public async Task UpdateStatusAsync(int id, int status)
        {
            var chamado = await _context.Chamados.FindAsync(id);
            if (chamado != null)
            {
                chamado.Status = (Status)status;
                await _context.SaveChangesAsync();
            }
        }

        public async Task UpdatePrioridadeAsync(int id, int prioridade)
        {
            var chamado = await _context.Chamados.FindAsync(id);
            if (chamado != null)
            {
                chamado.Prioridade = (Prioridade)prioridade;
                await _context.SaveChangesAsync();
            }
        }

        public async Task UpdateTituloAsync(int id, string titulo)
        {
            var chamado = await _context.Chamados.FindAsync(id);
            if (chamado != null)
            {
                chamado.Titulo = titulo;
                await _context.SaveChangesAsync();
            }
        }

        public async Task UpdateDescricaoAsync(int id, string descricao)
        {
            var chamado = await _context.Chamados.FindAsync(id);
            if (chamado != null)
            {
                chamado.Descricao = descricao;
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteAsync(int id)
        {
            var chamado = await _context.Chamados.FindAsync(id);
            if (chamado != null)
            {
                _context.Chamados.Remove(chamado);
                await _context.SaveChangesAsync();
            }
        }

        private async Task<int> GerarProtocoloAsync()
        {
            const int min = 100000; // mínimo (inclusive) — ajuste se quiser outro formato
            const int max = 999999; // máximo (inclusive)

            const int maxAttempts = 10;
            for (int attempt = 0; attempt < maxAttempts; attempt++)
            {
                int protocolo = RandomNumberGenerator.GetInt32(min, max + 1);
                var exists = await _context.Chamados.AsNoTracking().AnyAsync(c => c.Protocolo == protocolo);
                if (!exists) return protocolo;
            }

            // Fallback: usa parte de ticks para garantir unicidade razoável
            int fallback = (int)(DateTime.UtcNow.Ticks % int.MaxValue);
            // garante que o fallback não colida (sempre converge)
            while (await _context.Chamados.AsNoTracking().AnyAsync(c => c.Protocolo == fallback))
            {
                fallback++;
            }
            return fallback;
        }

        private async Task<int> SelecionarTecnicoAsync()
        {
            // Seleciona técnico com base no rodízio
            var tecnicos = await _context.Tecnicos.ToListAsync();
            if (!tecnicos.Any())
                throw new Exception("Nenhum técnico cadastrado.");

            // Busca o último chamado e decide quem é o próximo técnico
            var ultimoChamado = await _context.Chamados
                .OrderByDescending(c => c.IdChamado)
                .FirstOrDefaultAsync();

            int proximoIndice = 0;
            if (ultimoChamado != null)
            {
                var ultimoTecIndex = tecnicos.FindIndex(t => t.Id == ultimoChamado.IdTecnico);
                proximoIndice = (ultimoTecIndex + 1) % tecnicos.Count;
            }

            var tecnicoSelecionado = tecnicos[proximoIndice];
            return tecnicoSelecionado.Id;
        }

        public async Task<Interacao> AddInteracaoAsync(int chamadoId, Interacao interacao)
        {
            var chamado = await _context.Chamados.FindAsync(chamadoId);
            if (chamado == null) throw new InvalidOperationException("Chamado não encontrado.");

            interacao.IdChamado = chamadoId;
            interacao.DataInteracao = DateTime.Now;
            // Persiste a interação
            _context.Set<Interacao>().Add(interacao);

            await _context.SaveChangesAsync();
            return interacao;
        }
    }
}