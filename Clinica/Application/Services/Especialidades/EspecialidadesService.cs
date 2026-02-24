using Microsoft.EntityFrameworkCore;
using TurnosClinica.Application.DTOs.Especialidades;
using TurnosClinica.Infrastructure.Data;
using TurnosClinica.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
            // Obtenemos la lista de consultorios que no estén eliminados, y si se proporciona un nombre, filtramos por institucion que contenga ese nombre
            var query = _context.Especialidades.AsNoTracking().Where(e => e.EliminadoEn == null);

            // Si se proporciona un nombre, filtramos por institucion que contenga ese nombre
            if (!string.IsNullOrEmpty(nombre))
            {
                var n = nombre.Trim();
                query = query.Where(e => e.Nombre.Contains(n));
            }

            // Proyectamos a EspecialidadResponse y obtenemos la lista
            var lista = await query
                .Select(e => new EspecialidadResponse
                {
                    Id = e.Id,
                    Nombre = e.Nombre
                })
                .ToListAsync();
            // Devolvemos la lista
            return lista;
        }


        public async Task<int> CreateAsync(CrearEspecialidadRequest request)
        {

            var nombre = (request.Nombre ?? "").Trim();
            if (string.IsNullOrWhiteSpace(nombre))
                throw new InvalidOperationException("El nombre es obligatorio.");

            //Obtenemos la lista de especialidades que no estén eliminados, y si se proporciona un nombre, filtramos por institucion que contenga ese nombre
            var existeEspecialidad = await _context.Especialidades.AnyAsync(e =>
            e.Nombre == nombre && e.EliminadoEn == null);

            // Si existe una especialidad con el mismo nombre, lanzamos una excepción
            if (existeEspecialidad)
                throw new InvalidOperationException("Ya existe esa especialidad.");

            //Si existe pero esta eliminada, lanzamos una excepción
            var existeEspecialidadEliminada = await _context.Especialidades.IgnoreQueryFilters()
                .AnyAsync(e => e.Nombre == nombre && e.EliminadoEn != null);
            if (existeEspecialidadEliminada)
                throw new InvalidOperationException("La especialidad existe. Está eliminada");

            // Creamos la nueva especialidad y la guardamos en la base de datos
            var especialidad = new Especialidad
            {
                Nombre = nombre,
            };

            _context.Especialidades.Add(especialidad);
            await _context.SaveChangesAsync();

            // Devolvemos el id de la nueva especialidad
            return especialidad.Id;
        }

        public async Task<EspecialidadResponse> GetByIdAsync(int id)
        {

            // Obtenemos la especialidad por id, asegurándonos de que no esté eliminada, y proyectamos a EspecialidadResponse
            var especialidad = await _context.Especialidades
                .AsNoTracking()
                .Where(e => e.Id == id && e.EliminadoEn == null)
                .Select(e => new EspecialidadResponse
                {
                    Id = e.Id,
                    Nombre = e.Nombre
                })
                .FirstOrDefaultAsync();

            // Si no se encuentra la especialidad, lanzamos una excepción
            if (especialidad == null)
                throw new KeyNotFoundException("No se encontró la especialidad.");
            // Devolvemos la especialidad encontrada
            return especialidad;
        }

        public async Task UpdateAsync(int id, UpdateEspecialidadesRequest request)
        {

            // Obtenemos la especialidad por id, asegurándonos de que no esté eliminada
            var especialidad = await _context.Especialidades
                .Where(e => e.Id == id && e.EliminadoEn == null)
                .FirstOrDefaultAsync();

            var nombre = (request.Nombre ?? "").Trim();
            if (string.IsNullOrWhiteSpace(nombre))
                throw new InvalidOperationException("El nombre es obligatorio.");

            // Si no se encuentra la especialidad, lanzamos una excepción
            if (especialidad == null)
                throw new KeyNotFoundException("No se encontró la especialidad.");

            // Buscamos si existe otra especialidad con el mismo nombre, excluyendo la especialidad que estamos actualizando
            var existeEspecialidad = await _context.Especialidades.AnyAsync(e =>
            e.Nombre == nombre && e.Id != id);
            // Si existe otra especialidad con el mismo nombre, lanzamos una excepción
            if (existeEspecialidad)
                throw new InvalidOperationException("Ya existe esa especialidad.");
            //Si existe pero esta eliminada, lanzamos una excepción
            var existeEspecialidadEliminada = await _context.Especialidades.IgnoreQueryFilters()
                .AnyAsync(e => e.Nombre == nombre && e.EliminadoEn != null);
            if (existeEspecialidadEliminada)
                throw new InvalidOperationException("La especialidad existe. Está eliminada");


            // Actualizamos el nombre de la especialidad y guardamos los cambios en la base de datos
            especialidad.Nombre = nombre;
            await _context.SaveChangesAsync();
        }


        public async Task SoftDeleteAsync(int id)
        {

            // Obtenemos la especialidad por id, asegurándonos de que no esté eliminada
            var especialidad = await _context.Especialidades
                .Where(e => e.Id == id && e.EliminadoEn == null)
                .FirstOrDefaultAsync();
            // Si no se encuentra la especialidad, lanzamos una excepción
            if (especialidad == null)
                throw new KeyNotFoundException("No se encontró la especialidad.");
            // Marcamos la especialidad como eliminada estableciendo la fecha de eliminación y guardamos los cambios en la base de datos
            especialidad.EliminadoEn = DateTime.UtcNow;
            await _context.SaveChangesAsync();

        }

    }
}
