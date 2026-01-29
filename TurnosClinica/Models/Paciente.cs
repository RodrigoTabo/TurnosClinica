using System.Reflection.Metadata.Ecma335;

namespace TurnosClinica.Models
{
    public class Paciente
    {
        public Guid Id { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public  DateTime FechaNacimiento { get; set; }
        public int DNI { get; set; }
        public HistoriaClinica? HistoriaClinica { get; set; }
        public List<Turno> Turnos { get; set; }
    }
}
