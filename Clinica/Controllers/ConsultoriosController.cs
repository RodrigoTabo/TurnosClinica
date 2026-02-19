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
        public async Task<ActionResult<List<ConsultorioResponse>>> Get(string? nombre)
        => await _consultorioService.ListarAsync(nombre);

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] CrearConsultorioRequest request)
        {
            var id = await _consultorioService.CrearAsync(request);
            return Created($"/api/consultorios/{id}", new { id });
        }

        [HttpGet]
        [Route("{Id}")]
        public async Task<ActionResult<ConsultorioResponse>> GetByIdAsync(int Id)
        {
            var consultorio = await _consultorioService.GetByIdAsync(Id);
            return Ok(consultorio);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromRoute] int id, [FromBody] UpdateConsultorioRequest request)
        {
            await _consultorioService.UpdateAsync(id, request);
            return NoContent();
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> SoftDeleteAsync([FromRoute] int id)
        {
            await _consultorioService.SoftDeleteAsync(id);
            return NoContent();
        }




    }
}
