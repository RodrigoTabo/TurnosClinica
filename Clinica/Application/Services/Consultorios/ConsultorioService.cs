using Microsoft.EntityFrameworkCore;
using TurnosClinica.Application.DTOs.Consultorios;
using TurnosClinica.Infrastructure.Data;
using TurnosClinica.Models;

namespace TurnosClinica.Application.Services.Consultorios
{
    public class ConsultorioService : IConsultorioService
    {
        private readonly TurnosDbContext _context;
        public ConsultorioService(TurnosDbContext context)
        {
            _context = context;
        }


        public async Task<List<ConsultorioResponse>> ListarAsync(string? nombre)
        {
            // Obtenemos la lista de consultorios que no estén eliminados, y si se proporciona un nombre, filtramos por institucion que contenga ese nombre
            var query = _context.Consultorios.AsNoTracking().Where(c => c.EliminadoEn == null);


            // Si se proporciona un nombre, filtramos por institucion que contenga ese nombre (ignorando mayúsculas y espacios en blanco)
            if (!string.IsNullOrWhiteSpace(nombre))
            {
                var n = nombre.Trim();
                query = query.Where(c => c.Institucion.Contains(n));
            }

            // Proyectamos los consultorios a ConsultorioResponse y los retornamos como una lista
            var lista = await query
                .Select(c => new ConsultorioResponse
                {
                    Id = c.Id,
                    Institucion = c.Institucion,
                    Altura = c.Altura,
                    Calle = c.Calle,
                    Ciudad = c.Ciudad.Nombre + ", "
                       + c.Ciudad.Provincia.Nombre + ", "
                       + c.Ciudad.Provincia.Pais.Nombre
                })
                .ToListAsync();
            // Retornamos la lista de consultorios
            return lista;
        }

        public async Task<int> CrearAsync(CrearConsultorioRequest request)
        {

            //Validamos que ingrese el nombre de la institucion y que no sea solo espacios en blanco
            var institucion = (request.Institucion ?? "").Trim();
            if (string.IsNullOrWhiteSpace(institucion))
                throw new InvalidOperationException("El nombre es obligatorio.");

            //Validamos si existe la ciudad
            var existeCiudad = await _context.Ciudades.AnyAsync(c => c.Id == request.CiudadId);
            // Si no existe, lanzamos una excepción
            if (!existeCiudad)
                throw new KeyNotFoundException("La ciudad no existe.");

            // Consultamos si ya existe un consultorio con la misma institucion
            var existeConsultorio = await _context.Consultorios.AnyAsync(c => c.Institucion == institucion);
            // Si existe, lanzamos una excepción
            if (existeConsultorio)
                throw new InvalidOperationException("La institucion ya esta registrada");

            // Consultamos si ya existe un consultorio con la misma institucion y esta eliminada
            var existeConsultorioEliminado = await _context.Consultorios
                .IgnoreQueryFilters()
                .AnyAsync(c => c.Institucion == institucion && c.EliminadoEn != null);
            // Si existe, lanzamos una excepción
            if (existeConsultorioEliminado)
                throw new InvalidOperationException("La institucion ya existe. Esta eliminada.");


            // Creamos el nuevo consultorio y lo guardamos en la base de datos
            var consultorio = new Consultorio
            {
                Institucion = institucion,
                Altura = request.Altura,
                Calle = request.Calle,
                CiudadId = request.CiudadId
            };

            await _context.Consultorios.AddAsync(consultorio);
            await _context.SaveChangesAsync();

            //Retornamos el Id del nuevo consultorio creado
            return consultorio.Id;
        }

        public async Task<ConsultorioResponse> GetByIdAsync(int id)
        {
            var consultorio = await _context.Consultorios
                .AsNoTracking()
                .Where(c => c.Id == id && c.EliminadoEn == null)
                .Select(c => new ConsultorioResponse
                {
                    Id = c.Id,
                    Institucion = c.Institucion,
                    Altura = c.Altura,
                    Calle = c.Calle,
                    CiudadId = c.CiudadId,
                    Ciudad = c.Ciudad.Nombre + ", "
                       + c.Ciudad.Provincia.Nombre + ", "
                       + c.Ciudad.Provincia.Pais.Nombre
                }).FirstOrDefaultAsync();

            //Si no se encuentra el consultorio, lanzamos una excepción
            if (consultorio is null)
                throw new KeyNotFoundException("Consultorio no encontrado.");
            return consultorio;

        }


        public async Task SoftDeleteAsync(int id)
        {
            //Buscamos el consultorio por Id y verificamos que no esté eliminado
            var consultorio = await _context.Consultorios.FirstOrDefaultAsync(c => c.Id == id && c.EliminadoEn == null);

            //Si no se encuentra el consultorio, lanzamos una excepción
            if (consultorio is null)
                throw new KeyNotFoundException("Consultorio no encontrado.");

            // Marcamos el consultorio como eliminado estableciendo la fecha de eliminación
            consultorio.EliminadoEn = DateTime.UtcNow;

            await _context.SaveChangesAsync();
        }


        public async Task UpdateAsync(int id, UpdateConsultorioRequest request)
        {

            //Validamos que ingrese el nombre de la institucion y que no sea solo espacios en blanco
            var institucion = (request.Institucion ?? "").Trim();
            if (string.IsNullOrWhiteSpace(institucion))
                throw new InvalidOperationException("El nombre es obligatorio.");

            //Byscamos el consultorio por Id y verificamos que no esté eliminado con softdelete
            var existeCiudad = await _context.Ciudades.AnyAsync(c => c.Id == request.CiudadId & c.EliminadoEn == null);
            // Si no existe, lanzamos una excepción
            if (!existeCiudad)
                throw new KeyNotFoundException("La ciudad no existe.");

            //Buscamos el consultorio por Id y verificamos que no esté eliminado
            var consultorio = await _context.Consultorios.FirstOrDefaultAsync(c => c.Id == id && c.EliminadoEn == null);
            //Si no se encuentra el consultorio, lanzamos una excepción
            if (consultorio is null)
                throw new KeyNotFoundException("Consultorio no encontrado.");

            //Buscamos la institucion en otros consultorios para evitar duplicados, excluyendo el consultorio actual
            var existeConsultorio = await _context.Consultorios.AnyAsync(c => c.Institucion == institucion && c.Id != id);
            // Si existe, lanzamos una excepción
            if (existeConsultorio)
                throw new InvalidOperationException("La institucion ya esta registrada");

            // Consultamos si ya existe un consultorio con la misma institucion y esta eliminada
            var existeConsultorioEliminado = await _context.Consultorios
                .IgnoreQueryFilters()
                .AnyAsync(c => c.Institucion == institucion && c.EliminadoEn != null);
            // Si existe, lanzamos una excepción
            if (existeConsultorioEliminado)
                throw new InvalidOperationException("La institucion ya existe. Esta eliminada.");


            // Actualizamos los datos del consultorio con los valores del request
            consultorio.Institucion = institucion;
            consultorio.Altura = request.Altura;
            consultorio.Calle = request.Calle;
            consultorio.CiudadId = request.CiudadId;
            await _context.SaveChangesAsync();
        }



    }
}
