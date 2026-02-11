using Microsoft.EntityFrameworkCore;
using TurnosClinica.Application.DTOs.Provincias;
using TurnosClinica.Infrastructure.Data;
using TurnosClinica.Models;

namespace TurnosClinica.Application.Services.Provincias
{
    public class ProvinciasService : IProvinciasService
    {

        private readonly TurnosDbContext _context;

        public ProvinciasService(TurnosDbContext context)
        {
            _context = context;
        }


        public async Task<int>CrearAsync(CrearProvinciaRequest request)
        {

            //Validar Pais
            var pais =  _context.Paises.Any(p => p.Id == request.PaisId);
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


        public async Task<List<ProvinciaResponse>> ListarAsync()
        {
            var query = _context.Provincias.AsNoTracking();

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

    }
}
