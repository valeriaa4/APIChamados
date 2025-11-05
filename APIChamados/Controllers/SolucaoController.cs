using APIChamados.Dtos;
using APIChamados.Models;
using APIChamados.Services;
using Microsoft.AspNetCore.Mvc;

namespace APIChamados.Controllers
{
    [ApiController]
    [Route("api/solucao")]
    public class SolucaoController
    {
        public readonly SolucaoService _solucaoService;

        public SolucaoController(SolucaoService solucaoService)
        {
            _solucaoService = solucaoService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var solucoes = await _solucaoService.GetAllSolucoesAsync();
            return new OkObjectResult(solucoes);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var solucao = await _solucaoService.GetSolucaoByIdAsync(id);
            if (solucao == null)
            {
                return new NotFoundResult();
            }
            return new OkObjectResult(solucao);
        }

        [HttpPost]
        public async Task<Solucao> CreateAsync([FromBody] SolucaoDto solucaoDto)
        {
            return await _solucaoService.AddSolucaoAsync(solucaoDto);
        }

        [HttpPatch]
        public async Task<IActionResult> UpdateDescricaoAsync([FromQuery] int id, [FromBody] string descricao)
        {
            await _solucaoService.UpdateDescricaoAsync(id, descricao);
            return new NoContentResult();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var existingSolucao = await _solucaoService.GetSolucaoByIdAsync(id);
            if (existingSolucao == null)
            {
                return new NotFoundResult();
            }
            await _solucaoService.DeleteSolucaoAsync(id);
            return new NoContentResult();
        }
    }
}
