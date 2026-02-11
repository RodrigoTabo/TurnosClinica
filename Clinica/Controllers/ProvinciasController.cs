using Microsoft.AspNetCore.Mvc;
using TurnosClinica.Application.DTOs.Provincias;
using TurnosClinica.Application.Services.Provincias;

namespace TurnosClinica.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProvinciasController : ControllerBase
    {

        private readonly IProvinciasService _provinciasService;
        public ProvinciasController(IProvinciasService provinciasService)
        {
            _provinciasService = provinciasService; 
        }

        [HttpGet]
        public async Task<ActionResult<List<ProvinciaResponse>>> Get()
        => await _provinciasService.ListarAsync();

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] CrearProvinciaRequest request)
        {
            var id = await _provinciasService.CrearAsync(request);
            return Created($"/api/provincias/{id}", new { id });
        }
    }
}
