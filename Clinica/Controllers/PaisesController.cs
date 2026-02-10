using Microsoft.AspNetCore.Mvc;
using TurnosClinica.Application.DTOs.Especialidades;
using TurnosClinica.Application.DTOs.Paises;
using TurnosClinica.Application.Exceptions;
using TurnosClinica.Application.Services.Paises;

namespace TurnosClinica.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaisesController : Controller
    {

        private readonly IPaisesService _paisesService;

        public PaisesController(IPaisesService paisesSerivce)
        {
            _paisesService = paisesSerivce;
        }


        [HttpGet]
        public async Task<ActionResult<List<PaisResponse>>> Get()
        => await _paisesService.ListarPais();


        [HttpPost]
        public async Task<ActionResult> Post([FromBody] CrearPaisRequest request)
        {
            try
            {
                var id = await _paisesService.CrearAsync(request);
                return Created($"/api/paises/{id}", new { id });
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
