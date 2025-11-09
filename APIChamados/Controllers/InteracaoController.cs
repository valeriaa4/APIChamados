using APIChamados.Dtos;
using APIChamados.Models;
using APIChamados.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections;

namespace APIChamados.Controllers
{
    [ApiController]
    [Route("api/interacao")]
    public class InteracaoController
    {
        public readonly InteracaoService _interacaoService;

        public InteracaoController(InteracaoService interacaoService)
        {
            _interacaoService = interacaoService;
        }

        [HttpGet]
        public async Task<IEnumerable> GetAllAsync()
        {
            var interacoes = await _interacaoService.GetAllInteracoesAsync();
            return interacoes;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var interacao = await _interacaoService.GetInteracaoByIdAsync(id);
            if (interacao == null)
            {
                return new NotFoundResult();
            }
            return new OkObjectResult(interacao);
        }

        [HttpPost]
        public async Task<Interacao> CreateAsync([FromBody] InteracaoDto interacaoDto)
        {
            return await _interacaoService.AddInteracaoAsync(interacaoDto);
        }

        [HttpPatch("/resposta")]
        public async Task<IActionResult> UpdateRespostaAsync([FromQuery] int id, [FromBody] string resposta)
        {
            await _interacaoService.UpdateRespostaAsync(id, resposta);
            return new NoContentResult();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var existingInteracao = await _interacaoService.GetInteracaoByIdAsync(id);
            if (existingInteracao == null)
            {
                return new NotFoundResult();
            }
            await _interacaoService.DeleteInteracaoAsync(id);
            return new NoContentResult();
        }

    }
}
