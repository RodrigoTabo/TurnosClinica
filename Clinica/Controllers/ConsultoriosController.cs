using Microsoft.AspNetCore.Mvc;
using TurnosClinica.Application.DTOs.Consultorios;
using TurnosClinica.Application.Services.Consultorios;

namespace TurnosClinica.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ConsultoriosController : ControllerBase
    {
        private readonly IConsultorioService _consultorioService;
        public ConsultoriosController(IConsultorioService consultorioService)
        {
            _consultorioService = consultorioService;
        }

        [HttpGet]
        public async Task<ActionResult<List<ConsultorioResponse>>> Get()
        => await _consultorioService.ListarAsync();

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] CrearConsultorioRequest request)
        {
            var id = await _consultorioService.CrearAsync(request);
            return Created($"/api/consultorios/{id}", new { id });
        }

    }
}
