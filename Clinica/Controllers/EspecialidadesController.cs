using Microsoft.AspNetCore.Mvc;
using TurnosClinica.Application.DTOs.Especialidades;
using TurnosClinica.Application.Services.Especialidades;

namespace TurnosClinica.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EspecialidadesController : ControllerBase
    {
        private readonly IEspecialidadesService _especialidadesService;

        public EspecialidadesController(IEspecialidadesService especialidadesService)
        {
            _especialidadesService = especialidadesService;
        }

        [HttpGet]
        public async Task<ActionResult<List<EspecialidadResponse>>> Get(string? nombre)
        => await _especialidadesService.ListarAsync(nombre);

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] CrearEspecialidadRequest request)
        {
            var id = await _especialidadesService.CreateAsync(request);
            return Created($"/api/especialidades/{id}", new { id });
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<EspecialidadResponse>> GetByIdAsync(int id)
        {
            var especialidad = await _especialidadesService.GetByIdAsync(id);
            return Ok(especialidad);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Put([FromRoute] int id, [FromBody] UpdateEspecialidadesRequest request)
        {
            await _especialidadesService.UpdateAsync(id, request);
            return NoContent();
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> SoftDeleteAsync([FromRoute] int id)
        {
            await _especialidadesService.SoftDeleteAsync(id);
            return NoContent();
        }


    }
}
