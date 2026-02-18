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

            var existeEstado = await _context.Estados.AnyAsync(e => e.Nombre == request.Nombre);

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

        public async Task<List<EstadoResponse>> ListarAsync(string? nombre)
        {
            var query = _context.Estados.AsNoTracking().Where(e => e.EliminadoEn == null);

            if (!string.IsNullOrWhiteSpace(nombre))
            {
                var n = nombre.Trim();
                query = query.Where(p => p.Nombre.Contains(n));
            }

            var lista = await query
                .Select(e => new EstadoResponse
                {
                    Id = e.Id,
                    Nombre = e.Nombre
                }).ToListAsync();

            return lista;
        }


        public async Task<EstadoResponse> GetByIdAsync(int id)
        {
            var estado = await _context.Estados.AsNoTracking()
                .Where(e => e.Id == id && e.EliminadoEn == null)
                .Select(e => new EstadoResponse
                {
                    Id = e.Id,
                    Nombre = e.Nombre
                }).FirstOrDefaultAsync();
            if (estado == null)
                throw new KeyNotFoundException("Estado no encontrado.");
            return estado;
        }

        public async Task UpdateAsync(int id, UpdateEstadoRequest request)
        {
            var estado = await _context.Estados.Where(e => e.Id == id && e.EliminadoEn == null).FirstOrDefaultAsync();

            if (estado == null)
                throw new KeyNotFoundException("Estado no encontrado.");

            var existeEstado = await _context.Estados.AnyAsync(e => e.Id != id && e.Nombre == request.Nombre);

            if (existeEstado)
                throw new InvalidOperationException("El estado ya esta registrado.");
            estado.Nombre = request.Nombre;
            await _context.SaveChangesAsync();
        }

        public async Task SoftDeleteAsync(int id)
        {
            var estado = await _context.Estados.Where(e => e.Id == id && e.EliminadoEn == null).FirstOrDefaultAsync();
            if (estado == null)
                throw new KeyNotFoundException("Estado no encontrado.");
            estado.EliminadoEn = DateTime.UtcNow;
            await _context.SaveChangesAsync();

        }
    }
}
