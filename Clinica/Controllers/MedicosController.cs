using Microsoft.AspNetCore.Mvc;
using TurnosClinica.Application.DTOs.Medicos;
using TurnosClinica.Application.DTOs.Turnos;
using TurnosClinica.Application.Exceptions;
using TurnosClinica.Application.Services.Medicos;

namespace TurnosClinica.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MedicosController : ControllerBase
    {
        private readonly IMedicosService _medicosService;

        public MedicosController(IMedicosService medicosService)
        {
            _medicosService = medicosService;
        }

        [HttpGet]
        public async Task<ActionResult<List<MedicoResponse>>> Get()
        => await _medicosService.ListarAsync();

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] CrearMedicoRequest request)
        {
            try
            {
                var id = await _medicosService.CrearAsync(request);
                return Created($"/api/medicos/{id}", new { id });
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

    }
}
