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
        public async Task<List<EstadoResponse>> Get()=> await _estadoService.ListarAsync();


        [HttpPost]
        public async Task<ActionResult> Post([FromBody] CrearEstadoRequest request)
        {
            var id = await _estadoService.CrearAsync(request);
            return Created($"/api/estados/{id}", new { id });
        }


    }
}
