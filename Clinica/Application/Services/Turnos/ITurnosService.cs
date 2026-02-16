using TurnosClinica.Application.DTOs.Turnos;

namespace TurnosClinica.Application.Services.Turnos
{
    public interface ITurnosService
    {

        Task<int> CrearAsync(CrearTurnoRequest request);
        Task<List<TurnoResponse>> ListarAsync(DateTime desde, DateTime hasta, int? medicoId = null);
        Task CambiarEstadoAsync(int turnoId, CambiarEstadoRequest request);

    }
}
