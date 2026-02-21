using Microsoft.EntityFrameworkCore;
using TurnosClinica.Application.DTOs.Turnos;
using TurnosClinica.Infrastructure.Data;
using TurnosClinica.Models;

namespace TurnosClinica.Application.Services.Turnos
{
    public class TurnosService : ITurnosService
    {

        private readonly TurnosDbContext _context;

        public TurnosService(TurnosDbContext context)
        {
            _context = context;
        }

        public Task CambiarEstadoAsync(int turnoId, CambiarEstadoRequest request)
        {
            throw new NotImplementedException();
        }

        public async Task<int> CrearAsync(CrearTurnoRequest request)
        {

            //Validamos si existen.
            var paciente = await _context.Pacientes.AnyAsync(p => p.Id == request.PacienteId);
            if (!paciente)
                throw new KeyNotFoundException("El paciente no existe.");

            var medico = await _context.Medicos.AnyAsync(p => p.Id == request.MedicoId);
            if (!medico)
                throw new KeyNotFoundException("El medico no existe.");

            var consultorio = await _context.Consultorios.AnyAsync(p => p.Id == request.ConsultorioId);
            if (!consultorio)
                throw new KeyNotFoundException("El consultorio no existe.");

            var estadoId = request.EstadoId ?? await _context.Estados
                .Where(e => e.Nombre == "Pendiente")
                .Select(e => e.Id)
                .FirstOrDefaultAsync();

            if (estadoId == 0)
                throw new InvalidOperationException("No existe el estado 'Pendiente' en la tabla Estados.");

            //Validamos el choque ¿Ya tiene turno?
            var existeChoqueMedico = await _context.Turnos.AnyAsync(t =>
            t.MedicoId == request.MedicoId &&
            t.FechaTurno == request.FechaTurno);

            if (existeChoqueMedico)
                throw new InvalidOperationException("El médico ya tiene un turno en ese horario.");

            var existeChoqueConsulturio = await _context.Turnos.AnyAsync(t =>
            t.ConsultorioId == request.ConsultorioId &&
            t.FechaTurno == request.FechaTurno);

            if (existeChoqueConsulturio)
                throw new InvalidOperationException("El consultorio ya esta ocupado en ese horario.");

            var existeChoquePaciente = await _context.Turnos.AnyAsync(t =>
            t.PacienteId == request.PacienteId &&
            t.FechaTurno == request.FechaTurno);

            if (existeChoquePaciente)
                throw new InvalidOperationException("El paciente ya tiene un turno en ese horario.");


            var turno = new Turno
            {
                PacienteId = request.PacienteId,
                MedicoId = request.MedicoId,
                ConsultorioId = request.ConsultorioId,
                FechaTurno = request.FechaTurno,
                EstadoId = estadoId,
                Fecha = DateOnly.FromDateTime(DateTime.Now)
            };

            _context.Turnos.Add(turno);
            await _context.SaveChangesAsync();

            return turno.Id;

        }

        public async Task<List<TurnoResponse>> ListarAsync(DateTime desde, DateTime hasta, int? medicoId = null)
        {

            var query = _context.Turnos.AsNoTracking();

            query = query.Where(t => t.FechaTurno >= desde && t.FechaTurno <= hasta);

            if (medicoId.HasValue)
                query = query.Where(t => t.MedicoId == medicoId.Value);

            var lista = await query
                .OrderBy(t => t.FechaTurno)
                .Select(t => new TurnoResponse
                {
                    Id = t.Id,
                    FechaTurno = t.FechaTurno,


                    PacienteId = t.PacienteId,
                    PacienteNombreCompleto = t.Paciente.Nombre + " " + t.Paciente.Apellido,

                    MedicoId = t.MedicoId,
                    MedicoNombreCompleto = t.Medico.Nombre + " " + t.Medico.Apellido,


                    Estado = t.Estado.Nombre,
                    Consultorio = t.Consultorio.Institucion,
                    TienePago = (t.Pago != null)


                }).ToListAsync();

            return lista;

        }

        public async Task<TurnoResponse> GetByIdAsync(int Id)
        {
            var turno = await _context.Turnos
                .AsNoTracking()
                .Where(t => t.Id == Id)
                .Select(t => new TurnoResponse
                {
                    Id = t.Id,
                    FechaTurno = t.FechaTurno,
                    PacienteId = t.PacienteId,
                    PacienteNombreCompleto = t.Paciente.Nombre + " " + t.Paciente.Apellido,
                    MedicoId = t.MedicoId,
                    MedicoNombreCompleto = t.Medico.Nombre + " " + t.Medico.Apellido,
                    Estado = t.Estado.Nombre,
                    Consultorio = t.Consultorio.Institucion,
                    TienePago = (t.Pago != null)
                }).FirstOrDefaultAsync();
            if (turno == null)
                throw new KeyNotFoundException("El turno no existe.");
            return turno;

        }

        public async Task UpdateAsync(int Id, UpdateTurnoRequest request)
        {
         var turno = await _context.Turnos.FindAsync(Id);
            if (turno == null)
                throw new KeyNotFoundException("El turno no existe.");
            //Validamos si existen.
            var paciente = await _context.Pacientes.AnyAsync(p => p.Id == request.PacienteId);
            if (!paciente)
                throw new KeyNotFoundException("El paciente no existe.");
            var medico = await _context.Medicos.AnyAsync(p => p.Id == request.MedicoId);
            if (!medico)
                throw new KeyNotFoundException("El medico no existe.");
            var consultorio = await _context.Consultorios.AnyAsync(p => p.Id == request.ConsultorioId);
            if (!consultorio)
                throw new KeyNotFoundException("El consultorio no existe.");
            var estado = await _context.Estados.AnyAsync(p => p.Id == request.EstadoId);
            if (!estado)
                throw new KeyNotFoundException("El estado no existe.");

            turno.EstadoId = request.EstadoId;
            turno.PacienteId = request.PacienteId;
            turno.MedicoId = request.MedicoId;
            turno.ConsultorioId = request.ConsultorioId;
            turno.FechaTurno = request.FechaTurno;

            await _context.SaveChangesAsync();

        }

        public async Task SoftDeleteAsync(int id)
        {
            var turno = await _context.Turnos.FindAsync(id);
            if (turno == null)
                throw new KeyNotFoundException("El turno no existe.");
            turno.EliminadoEn = DateTime.Now;
            await _context.SaveChangesAsync();

        }


    }
}
