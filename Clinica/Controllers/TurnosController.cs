using Microsoft.AspNetCore.Mvc;
using TurnosClinica.Application.DTOs.Turnos;
using TurnosClinica.Application.Exceptions;
using TurnosClinica.Application.Services.Turnos;
using TurnosClinica.Models;

namespace TurnosClinica.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class TurnosController : ControllerBase
    {

        private readonly ITurnosService _service;

        public TurnosController(ITurnosService service)
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
            return Created($"/api/turnos/{id}", new { id });
        }

        [HttpPatch("{id:int}/estado")]
        public async Task<ActionResult> PatchEstado(int id, [FromBody] CambiarEstadoRequest request)
        {
            await _service.CambiarEstadoAsync(id, request);
            return NoContent();
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<TurnoResponse>> GetByIdAsync(int id)
        {
            var turno = await _service.GetByIdAsync(id);
            return Ok(turno);
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<ActionResult> UpdateAsync(int id, [FromBody] UpdateTurnoRequest request)
        {
            await _service.UpdateAsync(id, request);
            return NoContent();
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult> SoftDeleteAsync(int id)
        {
            await _service.SoftDeleteAsync(id);
            return NoContent();
        }


    }
}
