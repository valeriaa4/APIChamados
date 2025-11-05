using APIChamados.Dtos;
using APIChamados.Models;
using APIChamados.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections;

namespace APIChamados.Controllers
{
    [ApiController]
    [Route("api/chamado")]
    public class ChamadoController
    {
        private readonly ChamadoService _chamadoService;
        public ChamadoController(ChamadoService chamadoService)
        {
            _chamadoService = chamadoService;
        }

        [HttpGet]
        public async Task<IEnumerable> GetAllAsync()
        {
            var chamados = await _chamadoService.GetAllChamadosAsync();
            return chamados;
        }

        [HttpGet("protocolo/{protocolo}")]
        public async Task<IActionResult> GetByProtocoloAsync(int protocolo)
        {
            var chamado = await _chamadoService.GetChamadoByProtocoloAsync(protocolo);
            if (chamado == null)
            {
                return new NotFoundResult();
            }
            return new OkObjectResult(chamado);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var chamado = await _chamadoService.GetChamadoByIdAsync(id);
            if (chamado == null)
            {
                return new NotFoundResult();
            }
            return new OkObjectResult(chamado);
        }

        [HttpGet("prioridade/{prioridade}")]
        public async Task<IActionResult> GetByPrioridadeAsync(int prioridade)
        {
            var chamados = await _chamadoService.GetChamadosByPrioridadeAsync(prioridade);
            return new OkObjectResult(chamados);
        }

        [HttpGet("status/{status}")]
        public async Task<IActionResult> GetByStatusAsync(int status)
        {
            var chamados = await _chamadoService.GetChamadosByStatusAsync(status);
            return new OkObjectResult(chamados);
        }

        [HttpPost]
        public async Task<Chamado> CreateAsync([FromBody] ChamadoDto chamadoDto)
        {
            return await _chamadoService.AddChamadoAsync(chamadoDto);
        }

        [HttpPatch("/status")]
        public async Task<IActionResult> UpdateStatusAsync([FromQuery] int id, [FromQuery] int status)
        {
            await _chamadoService.UpdateChamadoStatusAsync(id, status);
            return new NoContentResult();
        }

        [HttpPatch("/prioridade")]
        public async Task<IActionResult> UpdatePrioridadeAsync([FromQuery] int id, [FromQuery] int prioridade)
        {
            await _chamadoService.UpdateChamadoPrioridadeAsync(id, prioridade);
            return new NoContentResult();
        }

        [HttpPatch("/titulo")]
        public async Task<IActionResult> UpdateTituloAsync([FromQuery] int id, [FromBody] string titulo)
        {
            await _chamadoService.UpdateChamadoTituloAsync(id, titulo);
            return new NoContentResult();
        }

        [HttpPatch("/descricao")]
        public async Task<IActionResult> UpdateDescricaoAsync([FromQuery] int id, [FromBody] string descricao)
        {
            await _chamadoService.UpdateChamadoDescricaoAsync(id, descricao);
            return new NoContentResult();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            await _chamadoService.DeleteChamadoAsync(id);
            return new NoContentResult();
        }
    }
}
