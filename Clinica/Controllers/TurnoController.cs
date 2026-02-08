using Microsoft.AspNetCore.Mvc;
using TurnosClinica.Application.DTOs;
using TurnosClinica.Application.Services;
using TurnosClinica.Models;

namespace TurnosClinica.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class TurnoController : ControllerBase
    {

        private readonly ITurnosService _service;

        public TurnoController(ITurnosService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<List<TurnoResponse>>> Get([FromQuery] DateTime desde, [FromQuery] DateTime hasta, [FromQuery] int? medicoId)
       => await _service.ListarAsync(desde, hasta, medicoId);

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] CrearTurnoRequest request)
        {
            var id = await _service.CrearAsync(request);
            return CreatedAtAction(nameof(GetById), new { id }, null);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<TurnoResponse>> GetById(int id)
        {
            throw new NotImplementedException();
        }

        [HttpPatch("{id:int}/estado")]
        public async Task<ActionResult> PatchEstado(int id, [FromBody] CambiarEstadoRequest request)
        {
            await _service.CambiarEstadoAsync(id, request);
            return NoContent();
        }

    }
}
