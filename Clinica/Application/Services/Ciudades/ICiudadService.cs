using TurnosClinica.Application.DTOs.Ciudades;

namespace TurnosClinica.Application.Services.Ciudades
{
    public interface ICiudadService
    {
        Task<List<CiudadResponse>> ListarAsync(string? Nombre);
        Task<int> CrearAsync(CrearCiudadRequest request);
        Task<CiudadResponse> GetByIdAsync(int id);
        Task UpdateAsync(int id, UpdateCiudadRequest request);
        Task SoftDeleteAsync(int id);
    }
}
