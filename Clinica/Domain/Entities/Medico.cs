namespace TurnosClinica.Models
{
    public class Medico
    {

        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public int DNI { get; set; }
        public int EspecialidadId { get; set; }
        public Especialidad Especialidad { get; set; }

        public List<Turno> Turnos { get; set; } = new();
        public bool Activo { get; set; }

        public DateTime? EliminadoEn { get; set; }

    }
}
