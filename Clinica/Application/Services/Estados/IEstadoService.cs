using TurnosClinica.Application.DTOs.Estados;

namespace TurnosClinica.Application.Services.Estados
{
    public interface IEstadoService
    {
        Task<List<EstadoResponse>> ListarAsync(string? nombre);
        Task<int> CrearAsync(CrearEstadoRequest request);
        Task<EstadoResponse> GetByIdAsync(int id);
        Task UpdateAsync(int id, UpdateEstadoRequest request);
        Task SoftDeleteAsync(int id);

    }
}
