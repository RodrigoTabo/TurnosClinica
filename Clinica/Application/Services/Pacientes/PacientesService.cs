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


        public async Task<PacienteResponse> GetByIdAsync(Guid id)
        {
            var paciente = await _context.Pacientes
                .Where(p => p.Id == id)
                .Select(p => new PacienteResponse
                {
                    Id = p.Id,
                    Nombre = p.Nombre,
                    Apellido = p.Apellido,
                    FechaNacimiento = p.FechaNacimiento,
                    DNI = p.DNI,
                    EmailPrincipal = p.EmailPrincipal
                })
                .FirstOrDefaultAsync();
            if (paciente is null)
                throw new InvalidOperationException("No existe un paciente con el ID ingresado");
            return paciente;
        }

        public async Task UpdateAsync(Guid Id, UpdatePacienteRequest request)
        {
            var paciente = await _context.Pacientes.FindAsync(Id);
            if (paciente is null)
                throw new InvalidOperationException("No existe un paciente con el ID ingresado");
            var existeOtroPacienteConDni = await _context.Pacientes
                .AnyAsync(p => p.DNI == request.DNI && p.Id != Id);
            if (existeOtroPacienteConDni)
                throw new InvalidOperationException("Ya existe otro paciente con el DNI ingresado");
            paciente.Nombre = request.Nombre;
            paciente.Apellido = request.Apellido;
            paciente.FechaNacimiento = request.FechaNacimiento;
            paciente.DNI = request.DNI;
            paciente.EmailPrincipal = request.EmailPrincipal;
            await _context.SaveChangesAsync();
        }

        public async Task SoftDeleteAsync(Guid id)
        {
            var paciente = await _context.Pacientes.FindAsync(id);
            if (paciente is null)
                throw new InvalidOperationException("No existe un paciente con el ID ingresado");
            _context.Pacientes.Remove(paciente);
            await _context.SaveChangesAsync();
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
