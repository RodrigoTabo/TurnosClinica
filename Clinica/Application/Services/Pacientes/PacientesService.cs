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

            // Validar que no exista otro paciente con el mismo DNI
            var existePaciente = await _context.Pacientes.AnyAsync(p => p.DNI == request.DNI);
            //Si existe, lanzamos una excepción
            if (existePaciente)
                throw new InvalidOperationException("El paciente ya existe");

            // Si no existe, creamos el paciente y la guardamos en la base de datos
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
            // Retornamos el Id del Paciente creado
            return paciente.Id;

        }

        public async Task<PacienteSelectorItem> GetByDniAsync(string dni)
        {
        
            // Si el DNI no es nulo o vacío, buscamos el paciente por DNI y retornamos un objeto PacienteSelectorItem con su información básica
            var paciente = await _context.Pacientes
                .Where(p => p.DNI == dni)
                .Select(p => new PacienteSelectorItem
                {
                    Id = p.Id,
                    DNI = p.DNI,
                    Nombre = p.Nombre,
                    Apellido = p.Apellido
                })
                .FirstOrDefaultAsync();

            if (paciente is null)
                throw new InvalidOperationException("No existe un paciente con el DNI ingresado");

            // retornamos el paciente encontrado.
            return paciente;
        }


        public async Task<PacienteResponse> GetByIdAsync(Guid id)
        {
            // Buscamos el paciente por Id y retornamos un objeto PacienteResponse con su información completa
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

            //Si el paciente es nulo, lanzamos una excepción
            if (paciente is null)
                throw new InvalidOperationException("No existe un paciente con el ID ingresado");
            return paciente;
        }

        public async Task UpdateAsync(Guid Id, UpdatePacienteRequest request)
        {
            // Buscamos el paciente por Id, si no existe, lanzamos una excepción
            var paciente = await _context.Pacientes.FindAsync(Id);
            if (paciente is null)
                throw new InvalidOperationException("No existe un paciente con el ID ingresado");

            // Validamos que no exista otro paciente con el mismo DNI (excluyendo al paciente que estamos actualizando)
            var existeOtroPacienteConDni = await _context.Pacientes
                .AnyAsync(p => p.DNI == request.DNI && p.Id != Id);
            if (existeOtroPacienteConDni)
                throw new InvalidOperationException("Ya existe otro paciente con el DNI ingresado");

            // Si el paciente existe y no hay otro paciente con el mismo DNI, actualizamos su información y guardamos los cambios en la base de datos
            paciente.Nombre = request.Nombre;
            paciente.Apellido = request.Apellido;
            paciente.FechaNacimiento = request.FechaNacimiento;
            paciente.DNI = request.DNI;
            paciente.EmailPrincipal = request.EmailPrincipal;
            await _context.SaveChangesAsync();
        }

        public async Task SoftDeleteAsync(Guid id)
        {
            // Buscamos el paciente por Id, si no existe, lanzamos una excepción
            var paciente = await _context.Pacientes.FindAsync(id);
            if (paciente is null)
                throw new InvalidOperationException("No existe un paciente con el ID ingresado");
            _context.Pacientes.Remove(paciente);
            await _context.SaveChangesAsync();
        }


        public async Task<List<PacienteResponse>> ListarAsync(string? DNI, string? Nombre, string? Apellido)
        {
            // Creamos una consulta base que seleccione todos los pacientes que no han sido eliminados
            var query = _context.Pacientes.AsNoTracking().Where(p => p.EliminadoEn == null);

            // Si se proporciona un DNI, filtramos la consulta para que solo incluya pacientes cuyo DNI comience con el valor proporcionado (ignorando espacios en blanco al inicio y al final)
            if (!string.IsNullOrEmpty(DNI))
            {
                DNI = DNI.Trim();
                query = query.Where(p => p.DNI.StartsWith(DNI));
            }
            // Si se proporciona un Nombre, filtramos la consulta para que solo incluya pacientes cuyo Nombre contenga el valor proporcionado
            if (!string.IsNullOrEmpty(Nombre))
                query = query.Where(p => p.Nombre.Contains(Nombre));
            // Si se proporciona un Apellido, filtramos la consulta para que solo incluya pacientes cuyo Apellido contenga el valor proporcionado
            if (!string.IsNullOrEmpty(Apellido))
                query = query.Where(p => p.Apellido.Contains(Apellido));

            // Ordenamos la consulta por Apellido y luego por Nombre, seleccionamos los campos necesarios para crear una lista de objetos PacienteResponse y la retornamos
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
