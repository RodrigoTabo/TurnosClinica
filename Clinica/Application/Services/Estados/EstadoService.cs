using Microsoft.EntityFrameworkCore;
using TurnosClinica.Application.DTOs.Estados;
using TurnosClinica.Infrastructure.Data;
using TurnosClinica.Models;

namespace TurnosClinica.Application.Services.Estados
{
    public class EstadoService : IEstadoService
    {

        private readonly TurnosDbContext _context;
        public EstadoService(TurnosDbContext context)
        {
            _context = context;
        }

        public async Task<int> CrearAsync(CrearEstadoRequest request)
        {

            var existeEstado =  await _context.Estados.AnyAsync(e => e.Nombre == request.Nombre);

            if (existeEstado)
                throw new InvalidOperationException("El estado ya esta registrado.");

            var estado = new Estado
            {
                Nombre = request.Nombre
            };

            _context.Estados.Add(estado);
            
            await _context.SaveChangesAsync();

            return estado.Id;

        }

        public async Task<List<EstadoResponse>> ListarAsync()
        {
            var query = _context.Estados.AsNoTracking();

            var lista = await query
                .Select(e => new EstadoResponse
                {
                    Id = e.Id,
                    Nombre = e.Nombre
                }).ToListAsync();


            return lista;

        }
    }
}
