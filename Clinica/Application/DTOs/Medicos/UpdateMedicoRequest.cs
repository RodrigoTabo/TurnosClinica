namespace TurnosClinica.Application.DTOs.Medicos
{
    public class UpdateMedicoRequest
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public int DNI { get; set; }
        public int EspecialidadId { get; set; }
    }
}
