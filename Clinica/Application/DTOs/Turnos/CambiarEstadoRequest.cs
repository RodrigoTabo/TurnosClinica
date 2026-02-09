namespace TurnosClinica.Application.DTOs.Turnos
{
    public class CambiarEstadoRequest
    {
        public int EstadoId { get; set; }
        public string? Motivo { get; set; }
    }
}
