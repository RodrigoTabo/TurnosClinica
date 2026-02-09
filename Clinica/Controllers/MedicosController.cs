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
        public async Task<ActionResult<List<MedicoResponse>>> Get()
        => await _medicosService.ListarAsync();

    }
}
