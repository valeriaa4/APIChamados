using APIChamados.Dtos;
using APIChamados.Models;
using APIChamados.Repositories;

namespace APIChamados.Services
{
    public class TecnicoService
    {
        private readonly ITecnicoRepository _tecnicoRepository;

        public TecnicoService(ITecnicoRepository tecnicoRepository)
        {
            _tecnicoRepository = tecnicoRepository;
        }

        public async Task<IEnumerable<Tecnico>> GetAllTecnicosAsync()
        {
            return await _tecnicoRepository.GetAllAsync();
        }

        public async Task<Tecnico?> GetTecnicoByIdAsync(int id)
        {
            return await _tecnicoRepository.GetByIdAsync(id);
        }

        public async Task<Tecnico> AddTecnicoAsync(TecnicoDto tecnicoDto)
        {
            return await _tecnicoRepository.AddAsync(tecnicoDto);
        }

        public async Task<Tecnico?> GetByEmailAndSenhaAsync(string email, string senha)
        {
            // Validações
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("O email é obrigatório.");

            if (string.IsNullOrWhiteSpace(senha))
                throw new ArgumentException("A senha é obrigatória.");

            var tecnico = await _tecnicoRepository.GetByEmailAndSenhaAsync(email, senha);

            if (tecnico == null)
                throw new UnauthorizedAccessException("Email ou senha inválidos.");

            return tecnico;
        }

        public async Task UpdateTecnicoAsync(Tecnico tecnico)
        {
            
            await _tecnicoRepository.UpdateAsync(tecnico);
        }

        public async Task DeleteTecnicoAsync(int id)
        {
            await _tecnicoRepository.DeleteAsync(id);
        }
    }
}
