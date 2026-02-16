using Microsoft.EntityFrameworkCore;
using TurnosClinica.Application.DTOs.Pacientes;
using TurnosClinica.Infrastructure.Data;
using TurnosClinica.Models;

namespace TurnosClinica.Application.Services.Pacientes
{
    public class PacientesService : IPacientesService
    {

        private readonly TurnosDbContext _context;

        public PacientesService(TurnosDbContext context)
        {
            _context = context;
        }

        public async Task<Guid> CrearAsync(CrearPacienteRequest request)
        {
            var existePaciente = await _context.Pacientes.AnyAsync(p => p.DNI == request.DNI);
            if (existePaciente)
                throw new InvalidOperationException("El paciente ya existe");

            var paciente = new Paciente
            {
                Id = request.Id,
                Nombre = request.Nombre,
                Apellido = request.Apellido,
                FechaNacimiento = request.FechaNacimiento,
                DNI = request.DNI,
                EmailPrincipal = request.EmailPrincipal,
                FechaAlta = DateOnly.FromDateTime(DateTime.Now)
            };


            _context.Pacientes.Add(paciente);

            await _context.SaveChangesAsync();

            return paciente.Id;

        }

        public async Task<PacienteSelectorItem> GetByDniAsync(string dni)
        {


            dni = dni.Trim();

            var item = await _context.Pacientes
                .Where(p => p.DNI == dni)
                .Select(p => new PacienteSelectorItem
                {
                    Id = p.Id,
                    DNI = p.DNI,
                    Nombre = p.Nombre,
                    Apellido = p.Apellido
                })
                .FirstOrDefaultAsync();

            if (item is null)
                throw new InvalidOperationException("No existe un paciente con el DNI ingresado");

            return item;
        }

        public async Task<List<PacienteResponse>> ListarAsync(string? DNI, string? Nombre, string? Apellido)
        {

            var query = _context.Pacientes.AsNoTracking();

            if (!string.IsNullOrEmpty(DNI))
            {
                DNI = DNI.Trim();
                query = query.Where(p => p.DNI.StartsWith(DNI));
            }

            if (!string.IsNullOrEmpty(Nombre))
                query = query.Where(p => p.Nombre.Contains(Nombre));

            if (!string.IsNullOrEmpty(Apellido))
                query = query.Where(p => p.Apellido.Contains(Apellido));

            var lista = await query.OrderBy(p => p.Apellido).ThenBy(p => p.Nombre)
                .Select(p => new PacienteResponse
                {
                    Id = p.Id,
                    Nombre = p.Nombre,
                    Apellido = p.Apellido,
                    FechaNacimiento = p.FechaNacimiento,
                    DNI = p.DNI,
                    EmailPrincipal = p.EmailPrincipal
                }).ToListAsync();

            return lista;

        }
    }

}
