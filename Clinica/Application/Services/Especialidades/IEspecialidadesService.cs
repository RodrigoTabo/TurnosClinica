using TurnosClinica.Application.DTOs.Especialidades;

namespace TurnosClinica.Application.Services.Especialidades
{
    public interface IEspecialidadesService
    {

        Task<List<EspecialidadResponse>> ListarAsync(string? nombre);
        Task<int> CreateAsync(CrearEspecialidadRequest request);

        Task<EspecialidadResponse> GetByIdAsync(int id);

        Task UpdateAsync(int id, UpdateEspecialidadesRequest request);

        Task SoftDeleteAsync(int id);

    }
}
