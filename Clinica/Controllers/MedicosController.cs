using Microsoft.AspNetCore.Mvc;
using TurnosClinica.Application.DTOs.Medicos;
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
        public async Task<ActionResult<List<MedicoResponse>>> Get(string? nombre)
        => await _medicosService.ListarAsync(nombre);

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] CrearMedicoRequest request)
        {
            var id = await _medicosService.CrearAsync(request);
            return Created($"/api/medicos/{id}", new { id });
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<MedicoResponse>> GetByIdAsync(int id)
        {
            var medico = await _medicosService.GetByIdAsync(id);
            return Ok(medico);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromRoute] int id, [FromBody] UpdateMedicoRequest request)
        {
            await _medicosService.UpdateAsync(id, request);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> SoftDeleteAsync([FromRoute] int id)
        {
            await _medicosService.SoftDeleteAsync(id);
            return NoContent();
        }


    }
}
