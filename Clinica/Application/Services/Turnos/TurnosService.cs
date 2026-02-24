using Microsoft.EntityFrameworkCore;
using System.Buffers.Text;
using TurnosClinica.Application.DTOs.Turnos;
using TurnosClinica.Application.Services.Email;
using TurnosClinica.Infrastructure.Data;
using TurnosClinica.Models;

namespace TurnosClinica.Application.Services.Turnos
{
    public class TurnosService : ITurnosService
    {

        private readonly TurnosDbContext _context;
        private readonly IEmailService _emailService;
        private readonly IConfiguration _config;

        public TurnosService(TurnosDbContext context, IEmailService emailService, IConfiguration config)
        {
            _context = context;
            _emailService = emailService;
            _config = config;
        }

        private static readonly HashSet<DayOfWeek> DiasHabilitados = new()
        {
            DayOfWeek.Monday,
            DayOfWeek.Tuesday,
            DayOfWeek.Wednesday,
            DayOfWeek.Thursday,
            DayOfWeek.Friday
        };

        private static readonly TimeOnly HoraInicio = new(9, 0);
        private static readonly TimeOnly HoraFin = new(18, 0); // 18:00 finaliza turno
        private const int DuracionTurnoMinutos = 30;

        // Descanso opcional
        private static readonly TimeOnly? DescansoDesde = new(13, 0);
        private static readonly TimeOnly? DescansoHasta = new(14, 0);

        public Task CambiarEstadoAsync(int turnoId, CambiarEstadoRequest request)
        {
            throw new NotImplementedException();
        }

        public async Task<int> CrearAsync(CrearTurnoRequest request)
        {

            ValidarHorarioPermitido(request.FechaTurno);

            //Validamos si el paciente existe
            var paciente = await _context.Pacientes.AnyAsync(p => p.Id == request.PacienteId);
            if (!paciente)
                throw new KeyNotFoundException("El paciente no existe.");
            // Validamos si el medico existe
            var medico = await _context.Medicos.AnyAsync(p => p.Id == request.MedicoId);
            if (!medico)
                throw new KeyNotFoundException("El medico no existe.");

            //Validamos si el consultorio existe
            var consultorio = await _context.Consultorios.AnyAsync(p => p.Id == request.ConsultorioId);
            if (!consultorio)
                throw new KeyNotFoundException("El consultorio no existe.");
            // Si no se envía estado, asignamos "Pendiente"
            var estadoId = request.EstadoId ?? await _context.Estados
                .Where(e => e.Nombre == "Pendiente")
                .Select(e => e.Id)
                .FirstOrDefaultAsync();
            // Si no existe el estado "Pendiente", lanzamos error
            if (estadoId == 0)
                throw new InvalidOperationException("No existe el estado 'Pendiente' en la tabla Estados.");

            //Buscamos el medico, la fecha del turno y verificamos si ya tiene un turno asignado en ese horario
            var existeChoqueMedico = await _context.Turnos.AnyAsync(t =>
            t.MedicoId == request.MedicoId &&
            t.FechaTurno == request.FechaTurno
              && t.Estado.Nombre != "Expirado"
           && t.Estado.Nombre != "Cancelado");
            // Si el médico ya tiene un turno en ese horario, lanzamos error
            if (existeChoqueMedico)
                throw new InvalidOperationException("El médico ya tiene un turno en ese horario.");

            //Buscamos el consultorio, la fecha del turno y verificamos si ya tiene un turno asignado en ese horario
            var existeChoqueConsulturio = await _context.Turnos.AnyAsync(t =>
            t.ConsultorioId == request.ConsultorioId
            && t.FechaTurno == request.FechaTurno
            && t.Estado.Nombre != "Expirado"
            && t.Estado.Nombre != "Cancelado");
            //Si el consultorio ya tiene un turno en ese horario, lanzamos error
            if (existeChoqueConsulturio)
                throw new InvalidOperationException("El consultorio ya esta ocupado en ese horario.");

            //Buscamos el paciente, la fecha del turno y verificamos si ya tiene un turno asignado en ese horario
            var existeChoquePaciente = await _context.Turnos.AnyAsync(t =>
            t.PacienteId == request.PacienteId &&
            t.FechaTurno == request.FechaTurno
            && t.Estado.Nombre != "Expirado"
            && t.Estado.Nombre != "Cancelado");

            //Si el paciente ya tiene un turno en ese horario, lanzamos error
            if (existeChoquePaciente)
                throw new InvalidOperationException("El paciente ya tiene un turno en ese horario.");

            //Si todo esta Ok, creamos el nuevo turno, lo guardamos en la base de datos y devolvemos su Id
            var turno = new Turno
            {
                PacienteId = request.PacienteId,
                MedicoId = request.MedicoId,
                ConsultorioId = request.ConsultorioId,
                FechaTurno = request.FechaTurno,
                EstadoId = estadoId,
                Fecha = DateOnly.FromDateTime(DateTime.Now),
                TokenConfirmacion = Guid.NewGuid().ToString("N"),
                TokenConfirmacionExpiraEn = DateTime.UtcNow.AddHours(2), // o 24 hs
                VerificadoEn = null
            };



            _context.Turnos.Add(turno);
            await _context.SaveChangesAsync();


            var pacienteExistente = await _context.Pacientes
            .AsNoTracking()
            .Where(p => p.Id == request.PacienteId)
            .Select(p => new { p.Nombre, p.EmailPrincipal })
            .FirstAsync();

            var baseUrl = _config["App:PublicBaseUrl"] ?? "https://localhost:5001";
            var token = turno.TokenConfirmacion!;
            var url = $"{baseUrl}/turnos/verificar?turnoId={turno.Id}&token={Uri.EscapeDataString(token)}";

            var html = $@"
            <p>Hola {pacienteExistente.Nombre},</p>
            <p>Tu turno fue creado correctamente para el día <strong>{turno.FechaTurno:dd/MM/yyyy HH:mm}</strong>.</p>
            <p>Para confirmar el turno, hacé click en el siguiente enlace:</p>
            <p><a href='{url}'>Confirmar turno</a></p>
            <p>Este enlace vence en 2 horas.</p>";

            await _emailService.SendAsync(
                pacienteExistente.EmailPrincipal,
                "Confirmación de turno",
                html
            );

            return turno.Id;

        }

        public async Task<(bool Ok, string Mensaje)> VerificarTurnoAsync(int turnoId, string token)
        {
            if (string.IsNullOrWhiteSpace(token))
                throw new InvalidOperationException("Token inválido.");

            var turno = await _context.Turnos.FirstOrDefaultAsync(t => t.Id == turnoId);
            if (turno is null)
                throw new InvalidOperationException("El turno no existe.");

            // Ya verificado
            if (turno.VerificadoEn != null)
                throw new InvalidOperationException("Este turno ya fue verificado.");

            // Validar token
            if (string.IsNullOrWhiteSpace(turno.TokenConfirmacion) ||
                !string.Equals(turno.TokenConfirmacion, token, StringComparison.Ordinal))
            {
                throw new InvalidOperationException("Token inválido.");
            }

            // Validar expiración
            if (!turno.TokenConfirmacionExpiraEn.HasValue)
                throw new InvalidOperationException("Este turno no tiene token de verificación activo.");


            if (turno.TokenConfirmacionExpiraEn.Value < DateTime.UtcNow)
            {
                var estadoExpiradoId = await _context.Estados
                    .Where(e => e.Nombre == "Expirado")
                    .Select(e => e.Id)
                    .FirstOrDefaultAsync();

                if (estadoExpiradoId != 0)
                    turno.EstadoId = estadoExpiradoId;

                // Invalidar token igual
                turno.TokenConfirmacion = null;
                turno.TokenConfirmacionExpiraEn = null;

                await _context.SaveChangesAsync();

                throw new InvalidOperationException("El enlace de verificación expiró.");
            }

            // Buscar estado Verificado
            var estadoVerificadoId = await _context.Estados
                .Where(e => e.Nombre == "Verificado")
                .Select(e => e.Id)
                .FirstOrDefaultAsync();

            if (estadoVerificadoId == 0)
                throw new InvalidOperationException("No existe el estado Verificado.");

            // Confirmar
            turno.VerificadoEn = DateTime.UtcNow;
            turno.EstadoId = estadoVerificadoId;

            // Invalidar token para que no se reutilice
            turno.TokenConfirmacion = null;
            turno.TokenConfirmacionExpiraEn = null;

            await _context.SaveChangesAsync();

            return (true, "Turno verificado correctamente.");
        }

        public async Task<List<TurnoResponse>> ListarAsync(DateTime desde, DateTime hasta, int? medicoId = null)
        {
            // Guardamos en la variable "query" la consulta a la tabla Turnos, sin seguimiento de cambios (AsNoTracking)
            var query = _context.Turnos.AsNoTracking();
            // Si se envía un rango de fechas, filtramos los turnos para que solo queden los que tengan la fecha del turno dentro de ese rango
            query = query.Where(t => t.FechaTurno >= desde && t.FechaTurno <= hasta);

            // Si se envía un Id de médico, filtramos los turnos para que solo queden los que tengan ese Id de médico
            if (medicoId.HasValue)
                query = query.Where(t => t.MedicoId == medicoId.Value);

            // Ordenamos los turnos por fecha del turno (más próximos primero) y proyectamos cada turno a un TurnoResponse, que incluye el Id, la fecha del turno, el nombre completo del paciente,
            // el nombre completo del médico, el estado del turno, el consultorio y si tiene pago o no. Finalmente, convertimos la consulta a una lista y la devolvemos.
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

            //Obtenemos turnos mediante Id, para mostrar el detalle del turno, si el turno no existe, lanzamos una excepcion,
            // si el turno existe, proyectamos el turno a un TurnoResponse, que incluye el Id, la fecha del turno, el nombre completo del paciente,
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
            //Obtenemos el turno mediante el Id
            var turno = await _context.Turnos.FindAsync(Id);
            //Si el turno no existe, lanzamos una excepcion
            if (turno == null)
                throw new KeyNotFoundException("El turno no existe.");
            //Validamos si el paciente existe, y si no es asi, lanzamos una excepcion
            var paciente = await _context.Pacientes.AnyAsync(p => p.Id == request.PacienteId);
            if (!paciente)
                throw new KeyNotFoundException("El paciente no existe.");
            //Validamos si el medico existe, y si no es asi, lanzamos una excepcion
            var medico = await _context.Medicos.AnyAsync(p => p.Id == request.MedicoId);
            if (!medico)
                throw new KeyNotFoundException("El medico no existe.");
            //Validamos si el consultorio existe, y si no es asi, lanzamos una excepcion
            var consultorio = await _context.Consultorios.AnyAsync(p => p.Id == request.ConsultorioId);
            if (!consultorio)
                throw new KeyNotFoundException("El consultorio no existe.");
            //  Validamos si el estado existe, y si no es asi, lanzamos una excepcion
            var estado = await _context.Estados.AnyAsync(p => p.Id == request.EstadoId);
            if (!estado)
                throw new KeyNotFoundException("El estado no existe.");

            if (request.FechaTurno == default)
                throw new ArgumentException("La fecha del turno es obligatoria.");

            if (request.FechaTurno < DateTime.Now)
                throw new InvalidOperationException("No se puede asignar un turno en una fecha/hora pasada.");

            ValidarHorarioPermitido(request.FechaTurno);

            // Validamos que el nuevo horario del turno no genere conflictos con otros turnos del mismo médico, paciente o consultorio.
            // Para esto, buscamos en la tabla Turnos si existe algún turno (distinto al que estamos editando) que tenga el mismo médico, paciente o consultorio y la misma fecha del turno.
            // Si encontramos alguno, lanzamos una excepción indicando el conflicto correspondiente.
            // Conflictos (ignorando Expirado / Cancelado)
            var conflictoMedico = await _context.Turnos.AnyAsync(t =>
                t.Id != Id &&
                t.EliminadoEn == null &&
                t.MedicoId == request.MedicoId &&
                t.FechaTurno == request.FechaTurno &&
                t.Estado.Nombre != "Expirado" &&
                t.Estado.Nombre != "Cancelado");

            if (conflictoMedico)
                throw new InvalidOperationException("El médico ya tiene un turno asignado en esa fecha y hora.");

            var conflictoPaciente = await _context.Turnos.AnyAsync(t =>
                t.Id != Id &&
                t.EliminadoEn == null &&
                t.PacienteId == request.PacienteId &&
                t.FechaTurno == request.FechaTurno &&
                t.Estado.Nombre != "Expirado" &&
                t.Estado.Nombre != "Cancelado");

            if (conflictoPaciente)
                throw new InvalidOperationException("El paciente ya tiene un turno asignado en esa fecha y hora.");

            var conflictoConsultorio = await _context.Turnos.AnyAsync(t =>
                t.Id != Id &&
                t.EliminadoEn == null &&
                t.ConsultorioId == request.ConsultorioId &&
                t.FechaTurno == request.FechaTurno &&
                t.Estado.Nombre != "Expirado" &&
                t.Estado.Nombre != "Cancelado");

            if (conflictoConsultorio)
                throw new InvalidOperationException("El consultorio ya tiene un turno asignado en esa fecha y hora.");

            turno.EstadoId = request.EstadoId;
            turno.PacienteId = request.PacienteId;
            turno.MedicoId = request.MedicoId;
            turno.ConsultorioId = request.ConsultorioId;
            turno.FechaTurno = request.FechaTurno;

            await _context.SaveChangesAsync();

        }

        public async Task SoftDeleteAsync(int id)
        {

            //Obtenemos el turno mediante el Id, si el turno no existe, lanzamos una excepcion,
            //si el turno existe, asignamos la fecha y hora actual a la propiedad EliminadoEn del turno,
            //guardamos los cambios en la base de datos y devolvemos el Id del turno eliminado
            var turno = await _context.Turnos.FindAsync(id);
            if (turno == null)
                throw new KeyNotFoundException("El turno no existe.");
            turno.EliminadoEn = DateTime.Now;
            await _context.SaveChangesAsync();

        }

        public async Task<Guid> ObtenerOCrearPacientePorDniAsync(CrearTurnoConPacienteRequest request)
        {

            // Validamos que se envíe el DNI, ya que es necesario para buscar o crear el paciente. Si no se envía, lanzamos una excepción.
            if (string.IsNullOrWhiteSpace(request.DNI))
                throw new ArgumentException("El DNI es obligatorio.");

            // Buscamos en la tabla Pacientes si existe un paciente que tenga el mismo DNI y no esté eliminado (EliminadoEn == null).
            // Si existe, obtenemos su Id. Si no existe, el resultado será Guid.Empty.
            var pacienteId = await _context.Pacientes
                .Where(p => p.DNI == request.DNI && p.EliminadoEn == null)
                .Select(p => p.Id)
                .FirstOrDefaultAsync();

            // Si YA existe, devolvés ese Id
            if (pacienteId != Guid.Empty)
                return pacienteId;

            // Si NO existe, validamos que se envíen el nombre, apellido y email del paciente, ya que son necesarios para crear el nuevo paciente.
            // Si falta alguno, lanzamos una excepción indicando el campo faltante.
            if (string.IsNullOrWhiteSpace(request.Nombre))
                throw new ArgumentException("El nombre es obligatorio para crear el paciente.");

            if (string.IsNullOrWhiteSpace(request.Apellido))
                throw new ArgumentException("El apellido es obligatorio para crear el paciente.");

            if (string.IsNullOrWhiteSpace(request.EmailPrincipal))
                throw new ArgumentException("El email es obligatorio para crear el paciente.");

            //Si NO existe, creamos un nuevo paciente con los datos enviados, lo guardamos en la base de datos y devolvemos su Id.
            //Para esto, instanciamos un nuevo objeto Paciente, asignamos sus propiedades con los datos del request, lo agregamos al contexto, guardamos los cambios y devolvemos el Id generado.
            var paciente = new Paciente
            {
                Nombre = request.Nombre,
                Apellido = request.Apellido,
                FechaNacimiento = request.FechaNacimiento,
                DNI = request.DNI,
                EmailPrincipal = request.EmailPrincipal
            };

            _context.Pacientes.Add(paciente);

            try
            {
                await _context.SaveChangesAsync();
                return paciente.Id;
            }
            catch (DbUpdateException)
            {
                // por si otro proceso lo creó justo antes
                pacienteId = await _context.Pacientes
                    .Where(p => p.DNI == request.DNI && p.EliminadoEn == null)
                    .Select(p => p.Id)
                    .FirstOrDefaultAsync();

                if (pacienteId == Guid.Empty)
                    throw;

                return pacienteId;
            }
        }

        public async Task<int> CrearAsegurandoPacienteAsync(CrearTurnoConPacienteRequest request)
        {
            // Primero, intentamos obtener o crear el paciente utilizando el DNI proporcionado en el request. Esto se hace para asegurar que tenemos un paciente válido para asociar al turno.
            var pacienteId = await ObtenerOCrearPacientePorDniAsync(request);

            ValidarHorarioPermitido(request.FechaTurno);
            // Luego, con el Id del paciente (ya sea existente o recién creado), procedemos a crear el turno utilizando el método CrearAsync, pasando un nuevo CrearTurnoRequest con los datos necesarios.
            return await CrearAsync(new CrearTurnoRequest
            {
                PacienteId = pacienteId,
                MedicoId = request.MedicoId,
                ConsultorioId = request.ConsultorioId,
                FechaTurno = request.FechaTurno,
                EstadoId = request.EstadoId
            });
        }

        private void ValidarHorarioPermitido(DateTime fechaTurno)
        {
            // 1) Día habilitado
            if (!DiasHabilitados.Contains(fechaTurno.DayOfWeek))
                throw new InvalidOperationException("No se otorgan turnos en el día seleccionado.");

            var hora = TimeOnly.FromDateTime(fechaTurno);

            // 2) Dentro de horario de atención
            if (hora < HoraInicio || hora >= HoraFin)
                throw new InvalidOperationException("El horario está fuera del rango de atención.");

            // 3) Debe coincidir con la grilla (ej. cada 30 min)
            var minutosDesdeInicio = (int)(hora.ToTimeSpan() - HoraInicio.ToTimeSpan()).TotalMinutes;

            if (minutosDesdeInicio < 0 || minutosDesdeInicio % DuracionTurnoMinutos != 0)
                throw new InvalidOperationException("La hora seleccionada no coincide con una franja válida.");

            // 4) No puede caer dentro del descanso
            if (DescansoDesde.HasValue && DescansoHasta.HasValue)
            {
                var desde = DescansoDesde.Value;
                var hasta = DescansoHasta.Value;

                if (hora >= desde && hora < hasta)
                    throw new InvalidOperationException("El horario seleccionado está dentro del horario de descanso.");
            }
        }

        private List<TimeOnly> GenerarFranjasDelDia()
        {
            var franjas = new List<TimeOnly>();

            var actual = HoraInicio;

            while (actual < HoraFin)
            {
                // Saltear descanso
                var estaEnDescanso =
                    DescansoDesde.HasValue && DescansoHasta.HasValue &&
                    actual >= DescansoDesde.Value && actual < DescansoHasta.Value;

                if (!estaEnDescanso)
                    franjas.Add(actual);

                actual = actual.AddMinutes(DuracionTurnoMinutos);
            }

            return franjas;
        }

        public async Task<List<TimeOnly>> ObtenerHorariosDisponiblesAsync(int medicoId, int consultorioId, DateOnly fecha)
        {
            // Validar que el día sea hábil (opcional acá)
            if (!DiasHabilitados.Contains(fecha.DayOfWeek))
                return new List<TimeOnly>();

            var franjas = GenerarFranjasDelDia();

            var desde = fecha.ToDateTime(TimeOnly.MinValue);
            var hasta = fecha.ToDateTime(TimeOnly.MaxValue);
            // Obtener los horarios ocupados para el médico y el consultorio en ese día
            var ocupadosMedico = await _context.Turnos
                .AsNoTracking()
                .Where(t => t.MedicoId == medicoId
                         && t.FechaTurno >= desde
                         && t.FechaTurno <= hasta)
                .Select(t => t.FechaTurno)
                .ToListAsync();

            var ocupadosConsultorio = await _context.Turnos
                .AsNoTracking()
                .Where(t => t.ConsultorioId == consultorioId
                         && t.FechaTurno >= desde
                         && t.FechaTurno <= hasta)
                .Select(t => t.FechaTurno)
                .ToListAsync();

            var horasOcupadas = ocupadosMedico
                .Concat(ocupadosConsultorio)
                .Select(TimeOnly.FromDateTime)
                .ToHashSet();

            var disponibles = franjas
                .Where(h => !horasOcupadas.Contains(h))
                .ToList();

            return disponibles;
        }

    }
}
