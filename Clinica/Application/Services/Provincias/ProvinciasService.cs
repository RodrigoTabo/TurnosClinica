using Microsoft.EntityFrameworkCore;
using TurnosClinica.Application.DTOs.Provincias;
using TurnosClinica.Infrastructure.Data;
using TurnosClinica.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TurnosClinica.Application.Services.Provincias
{
    public class ProvinciasService : IProvinciasService
    {

        private readonly TurnosDbContext _context;

        public ProvinciasService(TurnosDbContext context)
        {
            _context = context;
        }


        public async Task<int> CrearAsync(CrearProvinciaRequest request)
        {

            // Validamos que el nombre ingresado no sea nulo o solo espacios en blanco, y si es así, lanzamos una excepción indicando que el nombre es obligatorio.
            var nombre = (request.Nombre ?? "").Trim();
            if (string.IsNullOrWhiteSpace(nombre))
                throw new InvalidOperationException("Nombre es obligatorio.");
            // Validamos que el Id del país sea mayor a 0, y si no lo es, lanzamos una excepción indicando que el país es obligatorio.
            var pais = _context.Paises.Any(p => p.Id == request.PaisId);
            if (!pais)
                throw new KeyNotFoundException("El Pais no esta registrado.");

            //validamos que no exista una provincia con el mismo nombre (sin soft delete), y si existe, lanzamos una excepción indicando que la provincia ya existe.
            var provincia = _context.Provincias.Any(p => p.Nombre == nombre && p.EliminadoEn == null);
            if (provincia)
                throw new InvalidOperationException("La provincia ya existe.");

            var provinciaEliminada = _context.Provincias.IgnoreQueryFilters().Any(p => p.Nombre == nombre && p.EliminadoEn != null);
            if (provinciaEliminada)
                throw new InvalidOperationException("La provincia ya existe, esta eliminada.");

            // Creamos el objeto provincia, lo agregamos al contexto y guardamos los cambios en la base de datos. Finalmente, retornamos el Id de la nueva provincia creada.
            var nuevaProvincia = new Provincia
            {
                Nombre = nombre,
                PaisId = request.PaisId,
            };

            _context.Provincias.Add(nuevaProvincia);
            await _context.SaveChangesAsync();

            return nuevaProvincia.Id;

        }


        public async Task<List<ProvinciaResponse>> ListarAsync(string? Nombre)
        {
            // Creamos una consulta para obtener una lista de provincias que no esten eliminadas
            var query = _context.Provincias.AsNoTracking().Where(p => p.EliminadoEn == null);
            // Si se proporciona un nombre, filtramos la consulta para incluir solo las provincias cuyo nombre contenga el valor proporcionado (ignorando espacios en blanco al inicio y al final).
            if (!string.IsNullOrWhiteSpace(Nombre))
            {
                var n = Nombre.Trim();
                query = query.Where(p => p.Nombre.Contains(n));
            }
            // Ejecutamos la consulta y proyectamos los resultados en una lista de objetos ProvinciaResponse, y la retornamos
            var lista = await query
                .Select(p => new ProvinciaResponse
                {
                    Id = p.Id,
                    Nombre = p.Nombre,
                    PaisId = p.PaisId,
                    Pais = p.Pais.Nombre
                })
                .ToListAsync();

            return lista;

        }

        public async Task<ProvinciaResponse> GetByIdAsync(int id)
        {

            //Buscamos la provincia por su Id, asegurandonos que no este eliminada, y si no la encuentra, arroja una excepción, y si no es asi, retorna la provincia encontrada.
            var provincia = await _context.Provincias
                .AsNoTracking()
                .Where(p => p.Id == id && p.EliminadoEn == null)
                .Select(p => new ProvinciaResponse
                {
                    Id = p.Id,
                    Nombre = p.Nombre,
                    PaisId = p.PaisId,
                    Pais = p.Pais.Nombre
                })
                .FirstOrDefaultAsync();
            if (provincia == null)
                throw new KeyNotFoundException("La provincia no existe.");
            return provincia;

        }

        public async Task SoftDeleteAsync(int id)
        {
            // Buscamos la provincia por su Id, asegurandonos de que no este eliminada, si no la encuentra, arroja una excepcion,
            // y si la encuentra, establecemos su propiedad EliminadoEn con la fecha y hora actual en formato UTC, y guardamos los cambios en la base de datos.
            var provincia = await _context.Provincias.FirstOrDefaultAsync(p => p.Id == id && p.EliminadoEn == null);
            if (provincia == null)
                throw new KeyNotFoundException("La provincia no existe.");

            provincia.EliminadoEn = DateTime.UtcNow;
            await _context.SaveChangesAsync();

        }



        public async Task UpdateAsync(int id, UpdateProvinciaRequest request)
        {

            //Validamos que el nombre ingresado no sea nulo o solo espacios en blanco, y si es así, lanzamos una excepción indicando que el nombre es obligatorio.
            var nombre = (request.Nombre ?? "").Trim();
            if (string.IsNullOrWhiteSpace(nombre))
                throw new InvalidOperationException("El nombre es obligatorio.");

            //Validamos que el Id del país sea mayor a 0, y si no lo es, lanzamos una excepción indicando que el país es obligatorio.
            if (request.PaisId <= 0)
                throw new InvalidOperationException("El pais es obligatorio.");

            // Validamos existencia provincia (sin soft delete), y si no la encuentra, arroja una excepción indicando que la provincia no fue encontrada.
            var provincia = await _context.Provincias
                .FirstOrDefaultAsync(x => x.Id == id && x.EliminadoEn == null);

            if (provincia is null)
                throw new KeyNotFoundException("Provincia no encontrada.");

            // Validamos existencia pais
            var existePais = await _context.Paises
                .AnyAsync(p => p.Id == request.PaisId);
            if (!existePais)
                throw new KeyNotFoundException("El pais no existe.");
            // Validamos choque
            var existeChoque = await _context.Provincias
                .AnyAsync(p => p.Id != id && p.Nombre == nombre && p.EliminadoEn == null);
            if (existeChoque)
                throw new InvalidOperationException("La provincia ya existe.");

            var existeEliminada = await _context.Provincias
                .IgnoreQueryFilters()
               .AnyAsync(p => p.Id != id && p.Nombre == nombre && p.EliminadoEn != null);
            if (existeEliminada)
                throw new InvalidOperationException("La provincia ya existe, esta eliminada.");

            provincia.Nombre = nombre;
            provincia.PaisId = request.PaisId;
            await _context.SaveChangesAsync();
        }

    }

}
