namespace TurnosClinica.Models
{
    public class Ciudad
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public int ProvinciaId { get; set; }
        public Provincia Provincia { get; set; }
        public List<Consultorio> Consultorios { get; set; } = new();

        public DateTime? EliminadoEn { get; set; }

    }
}
