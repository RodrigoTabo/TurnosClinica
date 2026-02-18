namespace TurnosClinica.Application.DTOs.Estados
{
    public class CrearEstadoRequest
    {
        public int Id { get; set; }
        public string Nombre { get; set; }

        public DateTime? EliminadoEn { get; set; }
    }
}
