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

            //Validar Pais
            var pais = _context.Paises.Any(p => p.Id == request.PaisId);
            if (!pais)
                throw new KeyNotFoundException("El Pais no esta registrado.");

            //Validamos el choque
            var provincia = _context.Provincias.Any(p => p.Nombre == request.Nombre);
            if (provincia)
                throw new InvalidOperationException("La provincia ya existe.");

            var nuevaProvincia = new Provincia
            {
                Nombre = request.Nombre,
                PaisId = request.PaisId,
            };

            _context.Provincias.Add(nuevaProvincia);
            await _context.SaveChangesAsync();

            return nuevaProvincia.Id;

        }


        public async Task<List<ProvinciaResponse>> ListarAsync(string? Nombre)
        {
            var query = _context.Provincias.AsNoTracking().Where(p => p.EliminadoEn == null);

            if (!string.IsNullOrWhiteSpace(Nombre))
            {
                var n = Nombre.Trim();
                query = query.Where(p => p.Nombre.Contains(n));
            }

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
            var provincia = await _context.Provincias.FirstOrDefaultAsync(p => p.Id == id && p.EliminadoEn == null);
            if (provincia == null)
                throw new KeyNotFoundException("La provincia no existe.");

            provincia.EliminadoEn = DateTime.UtcNow;
            await _context.SaveChangesAsync();

        }



        public async Task UpdateAsync(int id, UpdateProvinciaRequest request)
        {
            var nombre = (request.Nombre ?? "").Trim();
            if (string.IsNullOrWhiteSpace(nombre))
                throw new InvalidOperationException("El nombre es obligatorio.");

            if (request.PaisId <= 0)
                throw new InvalidOperationException("El pais es obligatorio.");

            // Validamos existencia provincia (sin soft delete)
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
                .AnyAsync(p => p.Id != id && p.Nombre == nombre);
            if (existeChoque)
                throw new InvalidOperationException("La provincia ya existe.");
            provincia.Nombre = nombre;
            provincia.PaisId = request.PaisId;
            await _context.SaveChangesAsync();
        }

    }

}
