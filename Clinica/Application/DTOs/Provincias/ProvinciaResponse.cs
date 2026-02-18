using TurnosClinica.Models;

namespace TurnosClinica.Application.DTOs.Provincias
{
    public class ProvinciaResponse
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public List<Ciudad> Ciudades { get; set; } = new();
        public int PaisId { get; set; }
        public string Pais { get; set; }
        public DateTime? Eliminadoen { get; set; }
    }
}
