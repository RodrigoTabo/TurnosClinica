using Microsoft.AspNetCore.Mvc;
using TurnosClinica.Application.DTOs.Especialidades;
using TurnosClinica.Application.DTOs.Turnos;
using TurnosClinica.Application.Exceptions;
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
        public async Task<ActionResult<List<EspecialidadResponse>>> Get()
        => await _especialidadesService.ListarAsync();

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] CrearEspecialidadRequest request)
        {
                var id = await _especialidadesService.CreateAsync(request);
                return Created($"/api/especialidades/{id}", new { id });
        }


    }
}
