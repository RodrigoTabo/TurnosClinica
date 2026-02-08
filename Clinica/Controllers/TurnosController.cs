using Microsoft.AspNetCore.Mvc;
using TurnosClinica.Application.DTOs;
using TurnosClinica.Application.Exceptions;
using TurnosClinica.Application.Services;
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
            try
            {
                var id = await _service.CrearAsync(request);
                return Created($"/api/turnos/{id}", new { id });
            }
            catch (NotFoundAppException ex)
            {
                return NotFound(Problem(title: ex.Message, statusCode: 404));
            }
            catch (ConflictAppException ex)
            {
                return Conflict(Problem(title: ex.Message, statusCode: 409));
            }
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
