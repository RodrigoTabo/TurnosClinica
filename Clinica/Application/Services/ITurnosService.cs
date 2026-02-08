using TurnosClinica.Application.DTOs;
using TurnosClinica.Models;

namespace TurnosClinica.Application.Services
{
    public interface ITurnosService
    {

        Task<int> CrearAsync(CrearTurnoRequest request);
        Task<List<TurnoResponse>> ListarAsync(DateTime desde, DateTime hasta, int? medicoId = null);
        Task CambiarEstadoAsync(int turnoId, CambiarEstadoRequest request);

    }
}
