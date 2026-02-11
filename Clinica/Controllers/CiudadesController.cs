using Microsoft.AspNetCore.Mvc;
using TurnosClinica.Application.DTOs.Ciudades;
using TurnosClinica.Application.Services.Ciudades;

namespace TurnosClinica.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CiudadesController : ControllerBase
    {

        private readonly ICiudadService _ciudadService;

        public CiudadesController(ICiudadService ciudadService)
        {
            _ciudadService = ciudadService; 
        }

        [HttpGet]
        public async Task<ActionResult<List<CiudadResponse>>> Get()
        => await _ciudadService.ListarAsync();

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] CrearCiudadRequest request)
        {
            var id = await _ciudadService.CrearAsync(request);
            return Created($"/api/ciudades/{id}", new { id });
        }


    }
}
