namespace TurnosClinica.Application.DTOs
{
    public class CambiarEstadoRequest
    {
        public int EstadoId { get; set; }
        public string? Motivo { get; set; }
    }
}
