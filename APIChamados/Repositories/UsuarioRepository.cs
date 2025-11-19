using APIChamados.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using static APIChamados.Data.ApplicationDBContext;

namespace APIChamados.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {

        private readonly AppDbContext _context;

        public UsuarioRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Usuario?> GetByIdAsync(int id)
        {
            return await _context.Usuarios.FindAsync(id);
        }

        public async Task<IEnumerable<Usuario>> GetAllAsync()
        {
            return await _context.Usuarios.ToListAsync();
        }

        public async Task<Usuario> AddAsync(Usuario usuario)
        {
            if (usuario == null)
                throw new ArgumentNullException(nameof(usuario));

            if (string.IsNullOrWhiteSpace(usuario.Email))
                throw new ArgumentException("O email é obrigatório.", nameof(usuario.Email));

            var emailAttr = new EmailAddressAttribute();
            if (!emailAttr.IsValid(usuario.Email))
                throw new ArgumentException("O email informado é inválido.", nameof(usuario.Email));

            var exists = await _context.Usuarios.AsNoTracking().AnyAsync(u => u.Email == usuario.Email);
            if (exists)
                throw new InvalidOperationException("Já existe um usuário cadastrado com este email.");

            _context.Usuarios.Add(usuario);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                throw new InvalidOperationException("Falha ao salvar usuário — email já cadastrado.", ex);
            }
            return usuario;
        }

        public async Task<Usuario> GetByEmailAsync(string email)
        {
            return await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task DeleteAsync(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario != null)
            {
                _context.Usuarios.Remove(usuario);
                await _context.SaveChangesAsync();
            }
        }
    }
}
