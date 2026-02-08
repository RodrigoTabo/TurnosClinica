namespace TurnosClinica.Models
{
    public class Pais
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public List<Provincia> Provincias { get; set; } = new();
    }
}
