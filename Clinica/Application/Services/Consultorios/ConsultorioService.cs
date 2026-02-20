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

            var query = _context.Consultorios.AsNoTracking().Where(c => c.EliminadoEn == null);

            if (!string.IsNullOrWhiteSpace(nombre))
            {
                var n = nombre.Trim();
                query = query.Where(c => c.Institucion.Contains(n));
            }

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

            return lista;
        }

        public async Task<int> CrearAsync(CrearConsultorioRequest request)
        {

            //Validamos existencia ciudad
            var existeCiudad = await _context.Ciudades.AnyAsync(c => c.Id == request.CiudadId);
            if (!existeCiudad)
                throw new KeyNotFoundException("La ciudad no existe.");

            //Validamos choque de consultorio
            var existeConsultorio = await _context.Consultorios.AnyAsync(c => c.Institucion == request.Institucion);
            if (existeConsultorio)
                throw new InvalidOperationException("La institucion ya esta registrada");

            var consultorio = new Consultorio
            {
                Institucion = request.Institucion,
                Altura = request.Altura,
                Calle = request.Calle,
                CiudadId = request.CiudadId
            };

            await _context.Consultorios.AddAsync(consultorio);

            await _context.SaveChangesAsync();

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
            if (consultorio is null)
                throw new KeyNotFoundException("Consultorio no encontrado.");
            return consultorio;

        }


        public async Task SoftDeleteAsync(int id)
        {
            var consultorio = await _context.Consultorios.FirstOrDefaultAsync(c => c.Id == id && c.EliminadoEn == null);

            if (consultorio is null)
                throw new KeyNotFoundException("Consultorio no encontrado.");

            consultorio.EliminadoEn = DateTime.UtcNow;

            await _context.SaveChangesAsync();
        }


        public async Task UpdateAsync(int id, UpdateConsultorioRequest request)
        {
            //Validamos existencia ciudad
            var existeCiudad = await _context.Ciudades.AnyAsync(c => c.Id == request.CiudadId);
            if (!existeCiudad)
                throw new KeyNotFoundException("La ciudad no existe.");
            var consultorio = await _context.Consultorios.FirstOrDefaultAsync(c => c.Id == id && c.EliminadoEn == null);
            if (consultorio is null)
                throw new KeyNotFoundException("Consultorio no encontrado.");
            //Validamos choque de consultorio
            var existeConsultorio = await _context.Consultorios.AnyAsync(c => c.Institucion == request.Institucion && c.Id != id);
            if (existeConsultorio)
                throw new InvalidOperationException("La institucion ya esta registrada");
            consultorio.Institucion = request.Institucion;
            consultorio.Altura = request.Altura;
            consultorio.Calle = request.Calle;
            consultorio.CiudadId = request.CiudadId;
            await _context.SaveChangesAsync();
        }



    }
}
