using Microsoft.EntityFrameworkCore;
using TurnosClinica.Application.DTOs.Ciudades;
using TurnosClinica.Infrastructure.Data;
using TurnosClinica.Models;

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

            //Validamos provincia 
            var existeProvincia = await _context.Provincias.AnyAsync(p => p.Id == request.ProvinciaId);
            if (!existeProvincia)
                throw new KeyNotFoundException("La provincia no existe");

            //Evitar choque
            var existeCiudad = await _context.Ciudades.AnyAsync(c => c.Nombre == request.Nombre);
            if (existeCiudad)
                throw new InvalidOperationException("La ciudad ya esta registrada");


            var nuevaCiudad = new Ciudad
            {
                Nombre = request.Nombre,
                ProvinciaId = request.ProvinciaId
            };

            await _context.Ciudades.AddAsync(nuevaCiudad);
            await _context.SaveChangesAsync();

            return nuevaCiudad.Id;

        }

        public async Task<List<CiudadResponse>> ListarAsync()
        {
            var query = _context.Ciudades.AsNoTracking();

            var lista = await query
            .Select(c => new CiudadResponse
            {
                Id = c.Id,
                Nombre = c.Nombre,
                Provincia = c.Provincia.Nombre + ", " + c.Provincia.Pais.Nombre,
            }).ToListAsync();

            return lista;

        }
    }
}
