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
            var nombre = (request.Nombre ?? "").Trim();
            if (string.IsNullOrWhiteSpace(nombre))
                throw new InvalidOperationException("El nombre es obligatorio.");

            if (request.ProvinciaId <= 0)
                throw new InvalidOperationException("La provincia es obligatoria.");

            var existeProvincia = await _context.Provincias.AnyAsync(p => p.Id == request.ProvinciaId);
            if (!existeProvincia)
                throw new KeyNotFoundException("La provincia no existe.");

            var existeCiudad = await _context.Ciudades.AnyAsync(c =>
                c.EliminadoEn == null && c.Nombre == nombre && c.ProvinciaId == request.ProvinciaId);

            if (existeCiudad)
                throw new InvalidOperationException("La ciudad ya está registrada.");

            var nuevaCiudad = new Ciudad
            {
                Nombre = nombre,
                ProvinciaId = request.ProvinciaId
            };

            _context.Ciudades.Add(nuevaCiudad);
            await _context.SaveChangesAsync();

            return nuevaCiudad.Id;
        }


        public async Task<CiudadResponse> GetByIdAsync(int id)
        {

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

            if (ciudad is null)
                throw new KeyNotFoundException("Ciudad no encontrada.");


            return ciudad;
        }

        public async Task<List<CiudadResponse>> ListarAsync(string? nombre)
        {
            var query = _context.Ciudades.AsNoTracking().Where(p => p.EliminadoEn == null);

            if (!string.IsNullOrWhiteSpace(nombre))
            {
                var n = nombre.Trim();
                query = query.Where(p => p.Nombre.Contains(n));
            }

            var lista = await query
            .Select(c => new CiudadResponse
            {
                Id = c.Id,
                Nombre = c.Nombre,
                ProvinciaId = c.ProvinciaId,
                Provincia = c.Provincia.Nombre + ", " + c.Provincia.Pais.Nombre
            }).ToListAsync();

            return lista;

        }

        public async Task SoftDeleteAsync(int id)
        {
            var ciudad = await _context.Ciudades.FirstOrDefaultAsync(c => c.Id == id && c.EliminadoEn == null);

            if (ciudad is null)
                throw new KeyNotFoundException("Ciudad no encontrado.");

            ciudad.EliminadoEn = DateTime.UtcNow;
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(int id, UpdateCiudadRequest request)
        {
            var nombre = (request.Nombre ?? "").Trim();

            if (string.IsNullOrWhiteSpace(nombre))
                throw new InvalidOperationException("El nombre es obligatorio.");

            if (request.ProvinciaId <= 0)
                throw new InvalidOperationException("La provincia es obligatoria.");

            // Validamos existencia ciudad (sin soft delete)
            var ciudad = await _context.Ciudades
                .FirstOrDefaultAsync(x => x.Id == id && x.EliminadoEn == null);

            if (ciudad is null)
                throw new KeyNotFoundException("Ciudad no encontrada.");

            // Validamos existencia provincia
            var existeProvincia = await _context.Provincias
                .AnyAsync(p => p.Id == request.ProvinciaId);

            if (!existeProvincia)
                throw new KeyNotFoundException("La provincia no existe.");

            ciudad.Nombre = nombre;
            ciudad.ProvinciaId = request.ProvinciaId;

            await _context.SaveChangesAsync();
        }

    }
}
