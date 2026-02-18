using Microsoft.AspNetCore.Mvc;
using TurnosClinica.Application.DTOs.Estados;
using TurnosClinica.Application.Services.Estados;

namespace TurnosClinica.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EstadosController : ControllerBase
    {

        private readonly IEstadoService _estadoService;

        public EstadosController(IEstadoService estadoService)
        {
            _estadoService = estadoService;
        }

        [HttpGet]
        public async Task<List<EstadoResponse>> Get(string? nombre) => await _estadoService.ListarAsync(nombre);


        [HttpPost]
        public async Task<ActionResult> Post([FromBody] CrearEstadoRequest request)
        {
            var id = await _estadoService.CrearAsync(request);
            return Created($"/api/estados/{id}", new { id });
        }


        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<EstadoResponse>> GetByIdAsync(int id)
        {
            var estado = await _estadoService.GetByIdAsync(id);
            return Ok(estado);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Put([FromRoute] int id, [FromBody] UpdateEstadoRequest request)
        {
            await _estadoService.UpdateAsync(id, request);
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> SoftDeleteAsync([FromRoute] int id)
        {
            await _estadoService.SoftDeleteAsync(id);
            return NoContent();

        }
    }
}
