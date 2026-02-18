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

        public async Task<List<EspecialidadResponse>> ListarAsync(string? nombre)
        {

            var query = _context.Especialidades.AsNoTracking().Where(e => e.EliminadoEn == null);

            if (!string.IsNullOrEmpty(nombre))
            {
                var n = nombre.Trim();
                query = query.Where(e => e.Nombre.Contains(n));
            }

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

        public async Task<EspecialidadResponse> GetByIdAsync(int id)
        {
            var especialidad = await _context.Especialidades
                .AsNoTracking()
                .Where(e => e.Id == id && e.EliminadoEn == null)
                .Select(e => new EspecialidadResponse
                {
                    Id = e.Id,
                    Nombre = e.Nombre
                })
                .FirstOrDefaultAsync();
            if (especialidad == null)
                throw new KeyNotFoundException("No se encontró la especialidad.");
            return especialidad;
        }

        public async Task UpdateAsync(int id, UpdateEspecialidadesRequest request)
        {
            var especialidad = await _context.Especialidades
                .Where(e => e.Id == id && e.EliminadoEn == null)
                .FirstOrDefaultAsync();
            if (especialidad == null)
                throw new KeyNotFoundException("No se encontró la especialidad.");
            //Validamos el choque
            var existeEspecialidad = await _context.Especialidades.AnyAsync(e =>
            e.Nombre == request.Nombre && e.Id != id);
            if (existeEspecialidad)
                throw new InvalidOperationException("Ya existe esa especialidad.");
            especialidad.Nombre = request.Nombre;
            await _context.SaveChangesAsync();
        }


        public async Task SoftDeleteAsync(int id)
        {
            var especialidad = await _context.Especialidades
                .Where(e => e.Id == id && e.EliminadoEn == null)
                .FirstOrDefaultAsync();
            if (especialidad == null)
                throw new KeyNotFoundException("No se encontró la especialidad.");
            especialidad.EliminadoEn = DateTime.UtcNow;
            await _context.SaveChangesAsync();

        }

    }
}
