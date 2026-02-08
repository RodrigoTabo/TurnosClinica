using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using TurnosClinica.Application.DTOs;
using TurnosClinica.Infrastructure.Data;
using TurnosClinica.Models;

namespace TurnosClinica.Application.Services
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

            if(existeChoqueConsulturio)
            throw new InvalidOperationException("El consultorio ya esta ocupado en ese horario.");

            var existeChoquePaciente = await _context.Turnos.AnyAsync(t =>
            t.PacienteId == request.PacienteId &&
            t.FechaTurno == request.FechaTurno);

            if (existeChoquePaciente)
                throw new InvalidOperationException("El consultorio ya esta ocupado en ese horario.");


            var turno = new Turno
            {
                PacienteId = request.PacienteId,
                MedicoId = request.MedicoId,
                ConsultorioId = request.ConsultorioId,
                FechaTurno = request.FechaTurno,
                EstadoId = estadoId
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


    }
}
