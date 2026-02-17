using TurnosClinica.Application.DTOs.Paises;

namespace TurnosClinica.Application.Services.Paises
{
    public interface IPaisesService
    {
        Task<int> CrearAsync(CrearPaisRequest request);
        Task<List<PaisResponse>> ListarAsync(string? Nombre);

        Task<PaisResponse> GetByIdAsync(int id);
        Task UpdateAsync(int id, UpdatePaisRequest request);
        Task SoftDeleteAsync(int id);

    }
}
