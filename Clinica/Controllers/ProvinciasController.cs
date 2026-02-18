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
        public async Task<ActionResult<List<ProvinciaResponse>>> Get(string? Nombre)
        => await _provinciasService.ListarAsync(Nombre);

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] CrearProvinciaRequest request)
        {
            var id = await _provinciasService.CrearAsync(request);
            return Created($"/api/provincias/{id}", new { id });
        }

        [HttpGet]
        [Route("{Id}")]
        public async Task<ActionResult<ProvinciaResponse>> GetById(int Id)
        {
            var provincia = await _provinciasService.GetByIdAsync(Id);
            return Ok(provincia);
        }

        [HttpPut("{Id:int}")]
        public async Task<IActionResult> Put([FromRoute] int Id, [FromBody] UpdateProvinciaRequest request)
        {
            await _provinciasService.UpdateAsync(Id, request);
            return NoContent();
        }

        [HttpDelete]
        [Route("{Id}")]
        public async Task<IActionResult> Delete([FromRoute] int Id)
        {
            await _provinciasService.SoftDeleteAsync(Id);
            return NoContent();
        }

    }
}
