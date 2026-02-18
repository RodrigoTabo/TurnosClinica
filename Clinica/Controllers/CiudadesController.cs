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
        public async Task<ActionResult<List<CiudadResponse>>> Get(string? Nombre)
        => await _ciudadService.ListarAsync(Nombre);

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] CrearCiudadRequest request)
        {
            var id = await _ciudadService.CrearAsync(request);
            return Created($"/api/ciudades/{id}", new { id });
        }


        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<CiudadResponse>> GetByIdAsync(int id)
        {
            var ciudad = await _ciudadService.GetByIdAsync(id);
            return Ok(ciudad);
        }


        [HttpPut("{id:int}")]
        public async Task<IActionResult> Put([FromRoute] int id, [FromBody] UpdateCiudadRequest request)
        {
            await _ciudadService.UpdateAsync(id, request);
            return NoContent();
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            await _ciudadService.SoftDeleteAsync(id);
            return NoContent();
        }


    }
}
