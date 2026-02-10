using TurnosClinica.Application.DTOs.Provincias;

namespace TurnosClinica.Application.Services.Provincias
{
    public interface IProvinciasService
    {

        Task<List<ProvinciaResponse>> ListarAsync();

        Task<int> CrearAsync(CrearProvinciaRequest request);

    }
}
