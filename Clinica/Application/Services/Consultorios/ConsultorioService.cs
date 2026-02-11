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


        public async Task<List<ConsultorioResponse>> ListarAsync()
        {

            var query = _context.Consultorios.AsNoTracking();

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


    }
}
