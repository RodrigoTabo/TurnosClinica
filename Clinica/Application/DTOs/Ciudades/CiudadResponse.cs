
using TurnosClinica.Models;

namespace TurnosClinica.Application.DTOs.Ciudades
{
    public class CiudadResponse
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public int ProvinciaId { get; set; }
        public string Provincia { get; set; }
    }
}
