using Microsoft.EntityFrameworkCore;
using TurnosClinica.Application.DTOs.Estados;
using TurnosClinica.Infrastructure.Data;
using TurnosClinica.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
            var nombre = (request.Nombre ?? "").Trim();
            if (string.IsNullOrWhiteSpace(nombre))
                throw new InvalidOperationException("El nombre es obligatorio.");

            // Validar que no exista un estado con el mismo nombre
            var existeEstado = await _context.Estados.AnyAsync(e => e.Nombre == nombre);
            // Si existe, lanzar una excepción o manejarlo según la lógica de negocio
            if (existeEstado)
                throw new InvalidOperationException("El estado ya esta registrado.");


            //Existe pero esta eliminado? Sos gracioso...
            var existeEstadoEliminado = await _context.Estados
                .IgnoreQueryFilters()
                .AnyAsync(e => e.Nombre == nombre && e.EliminadoEn != null);
            if (existeEstadoEliminado)
                throw new InvalidOperationException("El estado existe. Está eliminado.");


            // Creamos la entidad Estado a partir del request y la guardamos en la base de datos
            var estado = new Estado
            {
                Nombre = request.Nombre
            };

            _context.Estados.Add(estado);
            await _context.SaveChangesAsync();
            // Retornar el Id del estado creado
            return estado.Id;

        }

        public async Task<List<EstadoResponse>> ListarAsync(string? nombre)
        {
            // Construir la consulta base para obtener los estados no eliminados
            var query = _context.Estados.AsNoTracking().Where(e => e.EliminadoEn == null);
            // Si se proporciona un nombre, agregar un filtro para buscar estados que contengan ese nombre
            if (!string.IsNullOrWhiteSpace(nombre))
            {
                var n = nombre.Trim();
                query = query.Where(p => p.Nombre.Contains(n));
            }
            // Ejecutar la consulta y proyectar los resultados a una lista de EstadoResponse
            var lista = await query
                .Select(e => new EstadoResponse
                {
                    Id = e.Id,
                    Nombre = e.Nombre
                }).ToListAsync();
            // Retornar la lista de estados
            return lista;
        }


        public async Task<EstadoResponse> GetByIdAsync(int id)
        {
            // Buscar el estado por su Id, asegurándose de que no esté eliminado, y proyectar el resultado a un EstadoResponse
            var estado = await _context.Estados.AsNoTracking()
                .Where(e => e.Id == id && e.EliminadoEn == null)
                .Select(e => new EstadoResponse
                {
                    Id = e.Id,
                    Nombre = e.Nombre
                }).FirstOrDefaultAsync();

            // Si no se encuentra el estado, lanzamos una excepcion
            if (estado == null)
                throw new KeyNotFoundException("Estado no encontrado.");
            // Retornar el estado encontrado
            return estado;
        }

        public async Task UpdateAsync(int id, UpdateEstadoRequest request)
        {

            var nombre = (request.Nombre ?? "").Trim();
            if (string.IsNullOrWhiteSpace(nombre))
                throw new InvalidCastException("El nombre es obligatorio.");


            // Buscamos el estado por su Id, asegurándonos de que no esté eliminado
            var estado = await _context.Estados.Where(e => e.Id == id && e.EliminadoEn == null).FirstOrDefaultAsync();
            // Si no se encuentra el estado, lanzamos una excepcion
            if (estado == null)
                throw new KeyNotFoundException("Estado no encontrado.");

            // Validar que no exista otro estado con el mismo nombre
            var existeEstado = await _context.Estados.AnyAsync(e => e.Id != id && e.Nombre == request.Nombre);
            // Si existe, lanzar una excepción o manejarlo según la lógica de negocio
            if (existeEstado)
                throw new InvalidOperationException("El estado ya esta registrado.");

            //Existe pero esta eliminado? Sos gracioso...
            var existeEstadoEliminado = await _context.Estados
                .IgnoreQueryFilters()
                .AnyAsync(e => e.Nombre == nombre && e.EliminadoEn != null);
            if (existeEstadoEliminado)
                throw new InvalidOperationException("El estado existe. Está eliminado.");

            // Actualizamos el nombre del estado con el valor del request y guardamos los cambios en la base de datos
            estado.Nombre = request.Nombre;
            await _context.SaveChangesAsync();
        }

        public async Task SoftDeleteAsync(int id)
        {
            // Buscamos el estado por su Id, asegurándonos de que no esté eliminado
            var estado = await _context.Estados.Where(e => e.Id == id && e.EliminadoEn == null).FirstOrDefaultAsync();
            // Si no se encuentra el estado, lanzamos una excepcion
            if (estado == null)
                throw new KeyNotFoundException("Estado no encontrado.");
            // Establecemos la fecha de eliminación del estado al momento actual y guardamos los cambios en la base de datos
            estado.EliminadoEn = DateTime.UtcNow;
            await _context.SaveChangesAsync();

        }
    }
}
