using TurnosClinica.Application.DTOs.Estados;

namespace TurnosClinica.Application.Services.Estados
{
    public interface IEstadoService
    {
        Task<List<EstadoResponse>> ListarAsync();
        Task<int> CrearAsync(CrearEstadoRequest request);

    }
}
