namespace TurnosClinica.Application.DTOs.Turnos
{
    public class CrearTurnoRequest
    {
        public Guid PacienteId { get; set; }
        public int MedicoId { get; set; }
        public int ConsultorioId { get; set; }
        public DateTime FechaTurno { get; set; }
        public int? EstadoId { get; internal set; }
    }
}
