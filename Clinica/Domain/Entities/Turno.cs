namespace TurnosClinica.Models
{
    public class Turno
    {
        public int Id { get; set; }
        public Guid PacienteId { get; set; }
        public Paciente Paciente { get; set; } = null!;
        public int MedicoId { get; set; }
        public Medico Medico { get; set; } = null!;
        public DateTime FechaTurno { get; set; }
        public int EstadoId { get; set; }
        public Estado Estado { get; set; }
        public int ConsultorioId { get; set; }
        public Consultorio Consultorio { get; set; }
        public Pago? Pago { get; set; }

    }
}
