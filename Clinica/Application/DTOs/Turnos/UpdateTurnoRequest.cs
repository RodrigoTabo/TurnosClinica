namespace TurnosClinica.Application.DTOs.Turnos
{
    public class UpdateTurnoRequest
    {
        public int Id { get; set; }
        public DateTime FechaTurno { get; set; }


        public Guid PacienteId { get; set; }
        public string PacienteNombreCompleto { get; set; } = "";

        public int MedicoId { get; set; }
        public string MedicoNombreCompleto { get; set; } = "";


        public int EstadoId { get; set; }
        public string Estado { get; set; } = "";

        public int ConsultorioId { get; set; }
        public string Consultorio { get; set; } = "";
        public bool TienePago { get; set; }
    }
}
