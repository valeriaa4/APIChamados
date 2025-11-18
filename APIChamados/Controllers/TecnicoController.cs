using APIChamados.Dtos;
using APIChamados.Models;
using APIChamados.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections;

namespace APIChamados.Controllers
{
    [ApiController]
    [Route("api/tecnico")]
    public class TecnicoController
    {
        public readonly TecnicoService _tecnicoService;

        public TecnicoController (TecnicoService tecnicoService)
        {
            _tecnicoService = tecnicoService;
        }

        [HttpGet]
        public async Task<IEnumerable> GetAllAsync()
        {
            var tecnicos = await _tecnicoService.GetAllTecnicosAsync();
            return tecnicos;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var tecnico = await _tecnicoService.GetTecnicoByIdAsync(id);
            if (tecnico == null)
            {
                return new NotFoundResult();
            }
            return new OkObjectResult(tecnico);
        }

        [HttpPost]
        public async Task<Tecnico> CreateAsync([FromBody] TecnicoDto tecnicoDto)
        {
            return await _tecnicoService.AddTecnicoAsync(tecnicoDto);
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync([FromBody] LoginDto loginDto)
        {
            try
            {
                var tecnico = await _tecnicoService.GetByEmailAndSenhaAsync(loginDto.Email, loginDto.Senha);
                return new OkObjectResult(tecnico);
            }
            catch (ArgumentException ex)
            {
                return new BadRequestObjectResult(new { message = ex.Message });
            }
            catch (UnauthorizedAccessException ex)
            {
                return new UnauthorizedObjectResult(new { message = ex.Message });
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateAsync([FromBody] Tecnico tecnico)
        {
            var existingTecnico = await _tecnicoService.GetTecnicoByIdAsync(tecnico.Id);
            if (existingTecnico == null)
            {
                return new NotFoundResult();
            }
            existingTecnico.Nome = tecnico.Nome;
            existingTecnico.Email = tecnico.Email;
            existingTecnico.Telefone = tecnico.Telefone;
            existingTecnico.Senha = tecnico.Senha;
            existingTecnico.Administrador = tecnico.Administrador;
            existingTecnico.DataContratacao = tecnico.DataContratacao;

            await _tecnicoService.UpdateTecnicoAsync(existingTecnico);
            return new NoContentResult();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var existingTecnico = await _tecnicoService.GetTecnicoByIdAsync(id);
            if (existingTecnico == null)
            {
                return new NotFoundResult();
            }
            await _tecnicoService.DeleteTecnicoAsync(id);
            return new NoContentResult();
        }
    }
}