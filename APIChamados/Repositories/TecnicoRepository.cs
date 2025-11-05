using APIChamados.Models;
using APIChamados.Dtos;
using Microsoft.EntityFrameworkCore;
using static APIChamados.Data.ApplicationDBContext;

namespace APIChamados.Repositories
{
    public class TecnicoRepository : ITecnicoRepository
    {
        public readonly AppDbContext _context;

        public TecnicoRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Tecnico>> GetAllAsync()
        {
            return await _context.Tecnicos.ToListAsync();
        }

        public async Task<Tecnico?> GetByIdAsync(int id)
        {
            return await _context.Tecnicos.FindAsync(id);
        }

        public async Task<Tecnico> AddAsync(TecnicoDto tecnicoDto)
        {
            var tecnico = new Tecnico
            {
                Nome = tecnicoDto.Nome,
                Telefone = tecnicoDto.Telefone,
                Email = tecnicoDto.Email,
                Senha = tecnicoDto.Senha,
                Administrador = tecnicoDto.Administrador,
                DataContratacao = tecnicoDto.DataContratacao
            };
            _context.Tecnicos.Add(tecnico);
            await _context.SaveChangesAsync();
            return tecnico;
        }

        public async Task<Tecnico> GetByEmailAndSenhaAsync(string email, string senha)
        {
            return await _context.Tecnicos.FirstOrDefaultAsync(t => t.Email == email && t.Senha == senha);
        }

        public async Task UpdateAsync(Tecnico tecnico)
        {
            _context.Tecnicos.Update(tecnico);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var tecnico = await _context.Tecnicos.FindAsync(id);
            if (tecnico != null)
            {
                _context.Tecnicos.Remove(tecnico);
                await _context.SaveChangesAsync();
            }
        }
    }
}
