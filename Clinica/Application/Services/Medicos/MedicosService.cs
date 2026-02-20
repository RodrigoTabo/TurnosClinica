using Azure.Core;
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

            var nombre = (request.Nombre ?? "").Trim();
            if (string.IsNullOrWhiteSpace(nombre))
                throw new InvalidOperationException("El nombre es obligatorio.");

            var apellido = (request.Apellido ?? "").Trim();
            if (string.IsNullOrWhiteSpace(apellido))
                throw new InvalidOperationException("El apellido es obligatorio.");

            var dni = request.DNI;
            if (dni == 0)
                throw new InvalidOperationException("El DNI es obligatorio.");

            //Validamos especialidad
            var especialidad = _context.Especialidades.Any(e => e.Id == request.EspecialidadId);
            if (!especialidad)
                throw new KeyNotFoundException("La especialidad no existe.");

            //Validamos choque
            var existedni = _context.Medicos.Any(m => m.DNI == request.DNI);
            if (existedni)
                throw new InvalidOperationException("El medico ya esta registrado");

            var medico = new Medico
            {
                Id = request.Id,
                Nombre = request.Nombre,
                Apellido = request.Apellido,
                DNI = request.DNI,
                EspecialidadId = request.EspecialidadId,
                Activo = true
            };

            _context.Medicos.Add(medico);
            await _context.SaveChangesAsync();

            return medico.Id;
        }

        public async Task<List<MedicoResponse>> ListarAsync(string nombre)
        {
            var query = _context.Medicos.AsNoTracking().Where(m => m.EliminadoEn == null);

            if (!string.IsNullOrWhiteSpace(nombre))
            {
                var n = nombre.Trim().ToLower();
                query = query.Where(m => m.Nombre.ToLower().Contains(n) || m.Apellido.ToLower().Contains(n));
            }

            var lista = await query
                .Select(m => new MedicoResponse
                {
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

        public async Task<MedicoResponse> GetByIdAsync(int id)
        {
            var medico = await _context.Medicos.AsNoTracking()
                .Where(m => m.Id == id && m.EliminadoEn == null)
                .Select(m => new MedicoResponse
                {
                    Id = m.Id,
                    Nombre = m.Nombre,
                    Apellido = m.Apellido,
                    DNI = m.DNI,
                    EspecialidadId = m.EspecialidadId,
                    Especialidad = m.Especialidad.Nombre
                })
                .FirstOrDefaultAsync();

            if (medico == null)
                throw new KeyNotFoundException("El medico no esta registrado");

            return medico;
        }

        public async Task UpdateAsync(int id, UpdateMedicoRequest request)
        {
            var medico = await _context.Medicos
                .Where(m => m.Id == id && m.EliminadoEn == null)
                .FirstOrDefaultAsync();

            var nombre = (request.Nombre ?? "").Trim();
            if (string.IsNullOrWhiteSpace(nombre))
                throw new InvalidOperationException("El nombre es obligatorio.");

            var apellido = (request.Apellido ?? "").Trim();
            if (string.IsNullOrWhiteSpace(apellido))
                throw new InvalidOperationException("El apellido es obligatorio.");

            var dni = request.DNI;
            if (dni == 0)
                throw new InvalidOperationException("El DNI es obligatorio.");

            //Validamos especialidad
            var especialidad = _context.Especialidades.Any(e => e.Id == request.EspecialidadId);
            if (!especialidad)
                throw new KeyNotFoundException("La especialidad no existe.");

            //Validamos choque
            var existedni = _context.Medicos.Any(m => m.DNI == request.DNI);
            if (existedni)
                throw new InvalidOperationException("El medico ya esta registrado");

            medico.Nombre = request.Nombre;
            medico.Apellido = request.Apellido;
            medico.DNI = request.DNI;
            medico.EspecialidadId = request.EspecialidadId;

            await _context.SaveChangesAsync();

        }


        public async Task SoftDeleteAsync(int id)
        {
            var medico = await _context.Medicos
                .Where(m => m.Id == id && m.EliminadoEn == null)
                .FirstOrDefaultAsync();
            if (medico == null)
                throw new KeyNotFoundException("El medico no esta registrado");
            medico.EliminadoEn = DateTime.UtcNow;
            await _context.SaveChangesAsync();
        }

    }
}
