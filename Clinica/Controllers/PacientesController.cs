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


    }
}
