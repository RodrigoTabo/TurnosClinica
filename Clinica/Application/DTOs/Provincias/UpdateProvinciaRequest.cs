namespace TurnosClinica.Application.DTOs.Provincias
{
    public class UpdateProvinciaRequest
    {
        public string Nombre { get; set; }
        public int PaisId { get; set; }
        public DateTime? Eliminadoen { get; set; }
    }
}
