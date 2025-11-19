using APIChamados.Models;
using APIChamados.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections;

namespace APIChamados.Controllers
{
    [ApiController]
    [Route("api/usuario")]
    public class UsuarioController
    {
        private readonly UsuarioService _usuarioService;

        public UsuarioController(UsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        [HttpGet]
        public async Task<IEnumerable> GetAllAsync()
        {
            var usuarios = await _usuarioService.GetAllUsuariosAsync();
            return usuarios;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var usuario = await _usuarioService.GetUsuarioByIdAsync(id);
            if (usuario == null)
            {
                return new NotFoundResult();
            }
            return new OkObjectResult(usuario);
        }

        [HttpGet("email/{email}")]
        public async Task<IActionResult> GetByEmailAsync(string email)
        {
            var usuario = await _usuarioService.GetUsuarioByEmailAsync(email);
            if (usuario == null)
            {
                return new NotFoundResult();
            }
            return new OkObjectResult(usuario);
        }

        [HttpPost]
        public async Task<Usuario> CreateAsync([FromBody] Usuario usuario)
        {
            return await _usuarioService.AddUsuarioAsync(usuario);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var existingUsuario = await _usuarioService.GetUsuarioByIdAsync(id);
            if (existingUsuario == null)
            {
                return new NotFoundResult();
            }
            await _usuarioService.DeleteUsuarioAsync(id);
            return new NoContentResult();
        }
    }
}
