using TurnosClinica.Models;

namespace TurnosClinica.Application.DTOs.Consultorios
{
    public class ConsultorioResponse
    {
        public int Id { get; set; }
        public string Institucion { get; set; }
        public string Calle { get; set; } = "";
        public int Altura { get; set; }
        public int CiudadId { get; set; }
        public string Ciudad { get; set; } = "";
    }
}
