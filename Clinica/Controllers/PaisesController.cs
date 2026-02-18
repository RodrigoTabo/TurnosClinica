using Microsoft.AspNetCore.Mvc;
using TurnosClinica.Application.DTOs.Paises;
using TurnosClinica.Application.Services.Paises;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TurnosClinica.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaisesController : ControllerBase
    {

        private readonly IPaisesService _paisesService;

        public PaisesController(IPaisesService paisesSerivce)
        {
            _paisesService = paisesSerivce;
        }


        [HttpGet]
        public async Task<ActionResult<List<PaisResponse>>> Get([FromQuery] string? Nombre)
        => await _paisesService.ListarAsync(Nombre);


        [HttpPost]
        public async Task<ActionResult> Post([FromBody] CrearPaisRequest request)
        {
            var id = await _paisesService.CrearAsync(request);
            return Created($"/api/paises/{id}", new { id });

        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<PaisResponse>> GetById(int id)
        {
            var pais = await _paisesService.GetByIdAsync(id);
            return Ok(pais);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Put([FromRoute] int id, [FromBody] UpdatePaisRequest request)
        {
            await _paisesService.UpdateAsync(id, request);
            return NoContent();
        }


        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            await _paisesService.SoftDeleteAsync(id);
            return NoContent();
        }
    }
}
