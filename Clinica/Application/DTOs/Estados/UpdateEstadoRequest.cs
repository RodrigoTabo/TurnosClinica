namespace TurnosClinica.Application.DTOs.Estados
{
    public class UpdateEstadoRequest
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public DateTime? EliminadoEn { get; set; }
    }
}
