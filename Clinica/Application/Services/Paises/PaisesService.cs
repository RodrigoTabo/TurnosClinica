using Microsoft.EntityFrameworkCore;
using TurnosClinica.Application.DTOs.Paises;
using TurnosClinica.Infrastructure.Data;
using TurnosClinica.Models;

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
            //Validamos el choque

            var ExistePais = _context.Paises.Any(
                p => p.Nombre == request.Nombre);

            if (ExistePais)
                throw new InvalidOperationException("El Pais ya esta registrado");

            var pais = new Pais
            {
                Id = request.Id,
                Nombre = request.Nombre
            };

            _context.Paises.Add(pais);
            await _context.SaveChangesAsync();
            return pais.Id;
        }


        public async Task<List<PaisResponse>> ListarPais()
        {
            var query = _context.Paises.AsNoTracking();

            var lista = await query
                .Select(p => new PaisResponse
                {
                    Id = p.Id,
                    Nombre = p.Nombre
                })
                .ToListAsync();

            return lista;

        }

    }
}
