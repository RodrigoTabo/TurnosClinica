using TurnosClinica.Application.DTOs.Provincias;

namespace TurnosClinica.Application.Services.Provincias
{
    public interface IProvinciasService
    {

        Task<List<ProvinciaResponse>> ListarAsync(string? Nombre);

        Task<int> CrearAsync(CrearProvinciaRequest request);

        Task<ProvinciaResponse> GetByIdAsync(int id);

        Task UpdateAsync (int id, UpdateProvinciaRequest response);

        Task SoftDeleteAsync (int id);



    }
}
