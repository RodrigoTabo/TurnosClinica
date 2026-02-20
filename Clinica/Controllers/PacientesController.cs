using Microsoft.AspNetCore.Mvc;
using TurnosClinica.Application.DTOs.Pacientes;
using TurnosClinica.Application.Services.Pacientes;

namespace TurnosClinica.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PacientesController : ControllerBase
    {
        private readonly IPacientesService _pacienteService;

        public PacientesController(IPacientesService pacientesService)
        {
            _pacienteService = pacientesService;
        }

        [HttpGet]
        public async Task<ActionResult<List<PacienteResponse>>> Get([FromQuery] string? DNI, [FromQuery] string? Nombre, [FromQuery] string? Apellido)
        => await _pacienteService.ListarAsync(DNI, Nombre, Apellido);

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] CrearPacienteRequest request)
        {
            var id = await _pacienteService.CrearAsync(request);
            return Created($"/api/pacientes/{id}", new { id });
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<PacienteResponse>> GetByIdAsync(Guid id)
        {
            var paciente = await _pacienteService.GetByIdAsync(id);
            return Ok(paciente);
        }

        [HttpGet("id/{dni}")]
        public async Task<ActionResult<PacienteResponse>> GetByDni([FromRoute] string dni)
        {
            var paciente = await _pacienteService.GetByDniAsync(dni);
            if (paciente == null)
                return NotFound();
            return Ok(paciente);
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<ActionResult> Put([FromRoute] Guid id, [FromBody] UpdatePacienteRequest request)
        {
            await _pacienteService.UpdateAsync(id, request);
            return NoContent();
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult> Delete([FromRoute] Guid id)
        {
            await _pacienteService.SoftDeleteAsync(id);
            return NoContent();
        }



    }
}
