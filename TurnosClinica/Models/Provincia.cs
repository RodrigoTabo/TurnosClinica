namespace TurnosClinica.Models
{
    public class Provincia
    {

        public int Id { get; set; }
        public string Nombre { get; set; }
        public List<Ciudad> Ciudades { get; set; }
        public int PaisId { get; set; }
        public Pais Pais { get; set; }

    }
}
