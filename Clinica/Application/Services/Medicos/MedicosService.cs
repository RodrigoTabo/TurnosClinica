using Microsoft.EntityFrameworkCore;
using TurnosClinica.Application.DTOs.Medicos;
using TurnosClinica.Infrastructure.Data;
using TurnosClinica.Models;

namespace TurnosClinica.Application.Services.Medicos
{
    public class MedicosService : IMedicosService
    {
        private readonly TurnosDbContext _context;

        public MedicosService(TurnosDbContext context)
        {
            _context = context;
        }

        public async Task<int> CrearAsync(CrearMedicoRequest request)
        {
            //Validamos especialidad
            var especialidad = _context.Especialidades.Any(e => e.Id == request.EspecialidadId);
            if (!especialidad)
                throw new KeyNotFoundException("La especialidad no existe.");

            //Validamos choque
            var dni = _context.Medicos.Any(m => m.DNI == request.DNI);
            if (dni)
                throw new InvalidOperationException("El medico ya esta registrado");

            var medico = new Medico
            {
                Id = request.Id,
                Nombre = request.Nombre,
                Apellido = request.Apellido,
                DNI = request.DNI,
                EspecialidadId = request.EspecialidadId
            };

            _context.Medicos.Add(medico);
            await _context.SaveChangesAsync();

            return medico.Id;
        }

        public async Task<List<MedicoResponse>> ListarAsync()
        {
            var query = _context.Medicos.AsNoTracking();


            var lista = await query
                .Select(m => new MedicoResponse {
                Id = m.Id,
                Nombre = m.Nombre,
                Apellido = m.Apellido,
                DNI = m.DNI,
                EspecialidadId = m.EspecialidadId,
                Especialidad = m.Especialidad.Nombre
                })
                .ToListAsync();

            return lista;

        }

    }
}
