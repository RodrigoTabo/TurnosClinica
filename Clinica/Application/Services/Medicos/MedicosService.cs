using Microsoft.EntityFrameworkCore;
using TurnosClinica.Application.DTOs.Medicos;
using TurnosClinica.Infrastructure.Data;

namespace TurnosClinica.Application.Services.Medicos
{
    public class MedicosService : IMedicosService
    {
        private readonly TurnosDbContext _context;

        public MedicosService(TurnosDbContext context)
        {
            _context = context;
        }

        public Task<int> CrearAsync(CrearMedicoRequest request)
        {
            //falta implementar la logica.
            throw new NotImplementedException();
        }

        public async Task<List<MedicoResponse>> ListarAsync()
        {
            var query = _context.Medicos.AsNoTracking();


            var lista = await query
                .Select(m => new MedicoResponse {
                Id = m.Id,
                Nombre = m.Nombre,
                Apellido = m.Apellido,
                DNI = m.DNI,
                EspecialidadId = m.EspecialidadId,
                Especialidad = m.Especialidad.Nombre
                })
                .ToListAsync();

            return lista;

        }

    }
}
