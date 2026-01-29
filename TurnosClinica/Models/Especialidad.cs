namespace TurnosClinica.Models
{
    public class Especialidad
    {

        public int Id { get; set; }
        public string Nombre { get; set; }

        public List<Medico> Medicos { get; set; }
    }
}
