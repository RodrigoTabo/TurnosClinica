using TurnosClinica.Application.DTOs.Ciudades;

namespace TurnosClinica.Application.Services.Ciudades
{
    public interface ICiudadService
    {
        Task<List<CiudadResponse>> ListarAsync();
        Task<int> CrearAsync(CrearCiudadRequest request);
    }
}
