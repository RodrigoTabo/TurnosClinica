namespace TurnosClinica.Models
{
    public class Paciente
    {
        public Guid Id { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public  DateTime FechaNacimiento { get; set; }
        public string DNI { get; set; }
        public string EmailPrincipal { get; set; }
        public string? EmailPendiente { get; set; }

        public HistoriaClinica? HistoriaClinica { get; set; }
        public List<Turno> Turnos { get; set; } = new();
    }
}
