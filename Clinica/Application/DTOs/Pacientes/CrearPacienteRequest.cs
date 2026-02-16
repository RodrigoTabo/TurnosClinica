namespace TurnosClinica.Application.DTOs.Pacientes
{
    public class CrearPacienteRequest
    {
        public Guid Id { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public string DNI { get; set; }
        public string EmailPrincipal { get; set; }

    }
}
