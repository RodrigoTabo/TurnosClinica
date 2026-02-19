using TurnosClinica.Application.DTOs.Consultorios;

namespace TurnosClinica.Application.Services.Consultorios
{
    public interface IConsultorioService
    {

        Task<List<ConsultorioResponse>> ListarAsync(string? nombre);
        Task<int> CrearAsync(CrearConsultorioRequest request);
        Task<ConsultorioResponse> GetByIdAsync(int id);
        Task UpdateAsync(int id, UpdateConsultorioRequest request);
        Task SoftDeleteAsync(int id);

    }
}
