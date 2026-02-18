using TurnosClinica.Models;

namespace TurnosClinica.Application.DTOs.Provincias
{
    public class CrearProvinciaRequest
    {
        public string Nombre { get; set; }
        public int PaisId { get; set; }
        public DateTime? Eliminadoen { get; set; } 
    }
}
