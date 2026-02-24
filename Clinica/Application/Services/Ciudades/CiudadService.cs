using Microsoft.EntityFrameworkCore;
using TurnosClinica.Application.DTOs.Ciudades;
using TurnosClinica.Infrastructure.Data;
using TurnosClinica.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TurnosClinica.Application.Services.Ciudades
{
    public class CiudadService : ICiudadService
    {

        private readonly TurnosDbContext _context;

        public CiudadService(TurnosDbContext context)
        {
            _context = context;
        }

        public async Task<int> CrearAsync(CrearCiudadRequest request)
        {
            //Validamos que ingrese el nombre y que no sea solo espacios en blanco
            var nombre = (request.Nombre ?? "").Trim();
            if (string.IsNullOrWhiteSpace(nombre))
                throw new InvalidOperationException("El nombre es obligatorio.");

            //Validamos que la provincia sea un id válido (mayor a 0) y que exista en la base de datos
            if (request.ProvinciaId <= 0)
                throw new InvalidOperationException("La provincia es obligatoria.");

            // Validamos existencia de la provincia y no este eliminada(SoftDelete)
            var existeProvincia = await _context.Provincias.AnyAsync(p => p.Id == request.ProvinciaId & p.EliminadoEn == null);
            // Si no existe la provincia, lanzamos una excepción
            if (!existeProvincia)
                throw new KeyNotFoundException("La provincia no existe.");

            //Validamos que no exista una ciudad con el mismo nombre en la misma provincia (sin soft delete)
            var existeCiudad = await _context.Ciudades.AnyAsync(c =>
                c.EliminadoEn == null && c.Nombre == nombre);

            // Si existe una ciudad con el mismo nombre en la misma provincia, lanzamos una excepción
            if (existeCiudad)
                throw new InvalidOperationException("La ciudad ya está registrada.");

            //Existe ciudad, pero esta eliminada
            var ciudadEliminada = await _context.Ciudades
            .IgnoreQueryFilters()
            .AnyAsync(c => c.Nombre == nombre && c.EliminadoEn != null);
            if (ciudadEliminada)
                throw new InvalidOperationException("La ciudad existe. Está eliminada.");

            //Si todo es válido, creamos la nueva ciudad y la guardamos en la base de datos
            var nuevaCiudad = new Ciudad
            {
                Nombre = nombre,
                ProvinciaId = request.ProvinciaId
            };

            _context.Ciudades.Add(nuevaCiudad);
            await _context.SaveChangesAsync();

            //Retornamos el id de la nueva ciudad creada
            return nuevaCiudad.Id;
        }


        public async Task<CiudadResponse> GetByIdAsync(int id)
        {


            //Buscamos la ciudad por su id, asegurándonos de que no esté eliminada (soft delete) y proyectamos a CiudadResponse
            var ciudad = await _context.Ciudades
                .AsNoTracking()
                .Where(c => c.Id == id && c.EliminadoEn == null)
                .Select(c => new CiudadResponse
                {
                    Id = c.Id,
                    Nombre = c.Nombre,
                    ProvinciaId = c.ProvinciaId,
                    Provincia = c.Provincia.Nombre + ", " + c.Provincia.Pais.Nombre
                }).FirstOrDefaultAsync();

            // Si no se encuentra la ciudad, lanzamos una excepción
            if (ciudad is null)
                throw new KeyNotFoundException("Ciudad no encontrada.");

            //Retornamos la ciudad encontrada proyectada a CiudadResponse
            return ciudad;
        }

        public async Task<List<CiudadResponse>> ListarAsync(string? nombre)
        {
            //Obtenemos todas las ciudades que no estén eliminadas (soft delete) y proyectamos a CiudadResponse
            var query = _context.Ciudades.AsNoTracking().Where(p => p.EliminadoEn == null);

            // Si se proporciona un nombre, filtramos las ciudades que contengan ese nombre (ignorando mayúsculas y minúsculas)
            if (!string.IsNullOrWhiteSpace(nombre))
            {
                var n = nombre.Trim();
                query = query.Where(p => p.Nombre.Contains(n));
            }

            //Obtener la lista de ciudades proyectada a CiudadResponse
            var lista = await query
            .Select(c => new CiudadResponse
            {
                Id = c.Id,
                Nombre = c.Nombre,
                ProvinciaId = c.ProvinciaId,
                Provincia = c.Provincia.Nombre + ", " + c.Provincia.Pais.Nombre
            }).ToListAsync();

            //Retornamos la lista de ciudades proyectada a CiudadResponse
            return lista;

        }

        public async Task SoftDeleteAsync(int id)
        {
            //Obtenemos la ciudad por su id, asegurándonos de que no esté eliminada (soft delete)
            var ciudad = await _context.Ciudades.FirstOrDefaultAsync(c => c.Id == id && c.EliminadoEn == null);

            // Si no se encuentra la ciudad, lanzamos una excepción
            if (ciudad is null)
                throw new KeyNotFoundException("Ciudad no encontrado.");

            // Si se encuentra la ciudad, establecemos la fecha de eliminación (soft delete) y guardamos los cambios en la base de datos
            ciudad.EliminadoEn = DateTime.UtcNow;
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(int id, UpdateCiudadRequest request)
        {
            // Validamos que el nombre no sea nulo, vacío o solo espacios en blanco
            var nombre = (request.Nombre ?? "").Trim();

            // Si el nombre es inválido, lanzamos una excepción
            if (string.IsNullOrWhiteSpace(nombre))
                throw new InvalidOperationException("El nombre es obligatorio.");

            // Si la provincia es menor o igual 0, lanzamos una excepción
            if (request.ProvinciaId <= 0)
                throw new InvalidOperationException("La provincia es obligatoria.");

            // Validamos existencia ciudad (con soft delete)
            var ciudad = await _context.Ciudades
                .FirstOrDefaultAsync(x => x.Id == id && x.EliminadoEn == null);
            // Si no se encuentra la ciudad, lanzamos una excepción
            if (ciudad is null)
                throw new KeyNotFoundException("Ciudad no encontrada.");

            // Validamos existencia provincia
            var existeProvincia = await _context.Provincias
                .AnyAsync(p => p.Id == request.ProvinciaId);
            //Si no existe la provincia, lanzamos una excepción
            if (!existeProvincia)
                throw new KeyNotFoundException("La provincia no existe.");

            // Validamos que no exista otra ciudad con el mismo nombre en la misma provincia (sin soft delete)
            var existeCiudad = await _context.Ciudades
            .AnyAsync(c => c.Nombre == request.Nombre);
            // Si existe otra ciudad con el mismo nombre en la misma provincia, lanzamos una excepción
            if (existeCiudad)
                throw new KeyNotFoundException("La ciudad ya esta registrada.");

            //Existe ciudad, pero esta eliminada
            var ciudadEliminada = await _context.Ciudades.IgnoreQueryFilters().AnyAsync(c => c.Nombre == nombre && c.EliminadoEn != null);
            if (ciudadEliminada)
                throw new InvalidOperationException("La ciudad existe. Está eliminada.");

            // Si todo es válido, actualizamos la ciudad con los nuevos datos y guardamos los cambios en la base de datos
            ciudad.Nombre = nombre;
            ciudad.ProvinciaId = request.ProvinciaId;

            await _context.SaveChangesAsync();
        }

    }
}
