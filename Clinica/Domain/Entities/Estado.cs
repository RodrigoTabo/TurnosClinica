namespace TurnosClinica.Models
{
    public class Estado
    {

        public int Id { get; set; }
        public string Nombre { get; set; }
        public List<Turno> Turnos { get; set; } = new();
    }
}
