using Microsoft.EntityFrameworkCore;
using TurnosClinica.Application.DTOs.Especialidades;
using TurnosClinica.Infrastructure.Data;
using TurnosClinica.Models;

namespace TurnosClinica.Application.Services.Especialidades
{
    public class EspecialidadesService : IEspecialidadesService
    {

        private readonly TurnosDbContext _context;

        public EspecialidadesService(TurnosDbContext context)
        {
            _context = context;
        }

        public async Task<List<EspecialidadResponse>> ListarAsync()
        {

            // Metodo que desactiva el seguimiento automático de cambios para los resultados de una consulta
            var query = _context.Especialidades.AsNoTracking();

            var lista = await query
                .Select(e => new EspecialidadResponse
                {
                    Id = e.Id,
                    Nombre = e.Nombre
                })
                .ToListAsync();

            return lista;
        }


        public async Task<int> CreateAsync(CrearEspecialidadRequest request)
        {

            //Validamos el choque
            var existeEspecialidad = await _context.Especialidades.AnyAsync(e =>
            e.Nombre == request.Nombre);

            if (existeEspecialidad)
                throw new InvalidOperationException("Ya existe esa especialidad.");

            var especialidad = new Especialidad
            {
                Nombre = request.Nombre,
            };

            _context.Especialidades.Add(especialidad);
            await _context.SaveChangesAsync();

            return especialidad.Id; 

        }
    }
}
