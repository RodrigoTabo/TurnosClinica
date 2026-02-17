using Microsoft.EntityFrameworkCore;
using TurnosClinica.Application.DTOs.Paises;
using TurnosClinica.Infrastructure.Data;
using TurnosClinica.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TurnosClinica.Application.Services.Paises
{
    public class PaisesService : IPaisesService
    {

        private readonly TurnosDbContext _context;

        public PaisesService(TurnosDbContext context)
        {
            _context = context;
        }

        public async Task<int> CrearAsync(CrearPaisRequest request)
        {
            //Nombre obligatorio
            var nombre = (request.Nombre ?? "").Trim();
            if (string.IsNullOrWhiteSpace(nombre))
                throw new InvalidOperationException("Nombre es obligatorio.");

            //Validamos el choque
            var existePais = await _context.Paises
                .AnyAsync(p => p.EliminadoEn == null && p.Nombre == request.Nombre);

            if (existePais)
                throw new InvalidOperationException("El Pais ya esta registrado");

            var pais = new Pais
            {
                Nombre = request.Nombre
            };

            _context.Paises.Add(pais);
            await _context.SaveChangesAsync();
            return pais.Id;
        }

        public async Task<PaisResponse> GetByIdAsync(int id)
        {
            var pais = await _context.Paises
                .AsNoTracking()
                .Where(x => x.Id == id && x.EliminadoEn == null)
                .Select(x => new PaisResponse { Id = x.Id, Nombre = x.Nombre })
                .FirstOrDefaultAsync();

            if (pais is null)
                throw new KeyNotFoundException("País no encontrado.");

            return pais;
        }


        public async Task<List<PaisResponse>> ListarAsync(string? nombre)
        {
            var query = _context.Paises.AsNoTracking().Where(p => p.EliminadoEn == null); ;

            if (!string.IsNullOrWhiteSpace(nombre))
            {
                var n = nombre.Trim();
                query = query.Where(p => p.Nombre.Contains(n));
            }

            var lista = await query.OrderBy(p => p.Nombre)
                .Select(p => new PaisResponse
                {
                    Id = p.Id,
                    Nombre = p.Nombre

                })
                .ToListAsync();

            return lista;

        }
        public async Task UpdateAsync(int id, UpdatePaisRequest request)
        {
            //nombre obligatorio
            var nombre = (request.Nombre ?? "").Trim();
            if (string.IsNullOrWhiteSpace(nombre))
                throw new ArgumentException("Nombre es obligatorio.");


            var existe = await _context.Paises
                .AnyAsync(p => p.Id != id && p.Nombre == nombre);

            if (existe)
                throw new InvalidOperationException("El País ya está registrado.");

            var pais = await _context.Paises
                .FirstOrDefaultAsync(x => x.Id == id && x.EliminadoEn == null);

            if (pais is null)
                throw new KeyNotFoundException("País no encontrado.");

            pais.Nombre = nombre;
            await _context.SaveChangesAsync();

        }

        public async Task SoftDeleteAsync(int id)
        {
            var pais = await _context.Paises
                .FirstOrDefaultAsync(x => x.Id == id && x.EliminadoEn == null);

            if (pais is null)
                throw new KeyNotFoundException("País no encontrado.");

            pais.EliminadoEn = DateTime.UtcNow;
            await _context.SaveChangesAsync();

        }

    }
}
