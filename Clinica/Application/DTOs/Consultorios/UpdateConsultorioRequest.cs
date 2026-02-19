namespace TurnosClinica.Application.DTOs.Consultorios
{
    public class UpdateConsultorioRequest
    {
        public string Institucion { get; set; }
        public string Calle { get; set; } = "";
        public int Altura { get; set; }
        public int CiudadId { get; set; }
    }
}
