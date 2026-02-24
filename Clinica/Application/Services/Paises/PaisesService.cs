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
            // Validamos que el nombre ingresado no sea nulo o solo espacios en blanco, y si es así, lanzamos una excepción indicando que el nombre es obligatorio.
            var nombre = (request.Nombre ?? "").Trim();
            if (string.IsNullOrWhiteSpace(nombre))
                throw new InvalidOperationException("Nombre es obligatorio.");

            //Validamos que el nombre del pais no exista, y si es asi, lanzamos una excepción indicando que el país ya está registrado.
            var existePais = await _context.Paises
                .AnyAsync(p => p.EliminadoEn == null && p.Nombre == nombre);
            if (existePais)
                throw new InvalidOperationException("El País ya esta registrado");
            // Validamos que el pais no exista, ni este eliminado
            var paisEliminado = await _context.Paises.IgnoreQueryFilters()
            .AnyAsync(p => p.Nombre == nombre && p.EliminadoEn != null);
            if (paisEliminado)
                throw new InvalidOperationException("El País existe. Está eliminado.");

            // Si el nombre es válido y no existe un país con el mismo nombre, lo creamos, lo guardamos en la base de datos y lo retornamos.
            var pais = new Pais
            {
                Nombre = nombre
            };

            _context.Paises.Add(pais);
            await _context.SaveChangesAsync();
            return pais.Id;
        }

        public async Task<PaisResponse> GetByIdAsync(int id)
        {

            //Buscamos el país por su Id, asegurándonos de que no esté eliminado, y si no lo encontramos lanzamos una excepcion, de lo contrario, retornamos un objeto PaisResponse con los datos del país encontrado.
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

            //Obtenemos una lista de países que no estén eliminados.
            var query = _context.Paises.AsNoTracking().Where(p => p.EliminadoEn == null);

            //Si se proporciona un nombre, filtramos la lista para incluir solo los paises
            if (!string.IsNullOrWhiteSpace(nombre))
            {
                var n = nombre.Trim();
                query = query.Where(p => p.Nombre.Contains(n));
            }

            //Ordenamos la lista por nombre, proyectamos cada país a un objeto PaisResponse y retornamos la lista resultante.
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
            //Validamos que el nombre ingresado no sea nulo o solo espacios en blanco, y si es así, lanzamos una excepción indicando que el nombre es obligatorio.
            var nombre = (request.Nombre ?? "").Trim();
            if (string.IsNullOrWhiteSpace(nombre))
                throw new ArgumentException("Nombre es obligatorio.");

            //Si existe el país con el mismo nombre pero con un Id diferente al que se está actualizando, lanzamos una excepción indicando que el país ya está registrado.
            var existe = await _context.Paises
                .AnyAsync(p => p.Id != id && p.Nombre == nombre);
            if (existe)
                throw new InvalidOperationException("El País ya está registrado.");
            // Buscamos el País por su Id, si no lo encontramos o esta eliminado, lanzamos una excepción
            var pais = await _context.Paises
                .FirstOrDefaultAsync(x => x.Id == id && x.EliminadoEn == null);
            if (pais is null)
                throw new KeyNotFoundException("País no encontrado.");

            // Validamos que el pais no exista, ni este eliminado
            var paisEliminado = await _context.Paises.AnyAsync(p => p.Nombre == request.Nombre && p.EliminadoEn != null);
            if (paisEliminado)
                throw new InvalidOperationException("El País existe. Está eliminado.");

            // Si el País existe, actualizamos su nombre, guardamos los cambios en la base de datos.
            pais.Nombre = nombre;
            await _context.SaveChangesAsync();

        }

        public async Task SoftDeleteAsync(int id)
        {
            // Buscamos el País por su Id, si no lo encontramos o esta eliminado, lanzamos una excepción
            var pais = await _context.Paises
                .FirstOrDefaultAsync(x => x.Id == id && x.EliminadoEn == null);

            if (pais is null)
                throw new KeyNotFoundException("País no encontrado.");
            // Si el País existe, establecemos su propiedad EliminadoEn con la fecha y hora actual en formato UTC, y guardamos los cambios en la base de datos.
            pais.EliminadoEn = DateTime.UtcNow;
            await _context.SaveChangesAsync();

        }

    }
}
