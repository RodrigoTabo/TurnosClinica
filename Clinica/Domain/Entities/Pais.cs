using TurnosClinica.Domain.Entities.Interfaces;

namespace TurnosClinica.Models
{
    public class Pais : ISoftDelete
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = "";
        public List<Provincia> Provincias { get; set; } = new();
        public DateTime? EliminadoEn { get ; set ; }
    }
}
