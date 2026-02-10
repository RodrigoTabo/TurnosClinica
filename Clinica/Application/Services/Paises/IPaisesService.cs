using TurnosClinica.Application.DTOs.Paises;

namespace TurnosClinica.Application.Services.Paises
{
    public interface IPaisesService
    {
        Task<int> CrearAsync(CrearPaisRequest request);
        Task<List<PaisResponse>> ListarPais();

    }
}
