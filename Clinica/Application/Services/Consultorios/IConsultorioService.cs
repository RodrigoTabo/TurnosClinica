using TurnosClinica.Application.DTOs.Consultorios;

namespace TurnosClinica.Application.Services.Consultorios
{
    public interface IConsultorioService
    {

        Task<List<ConsultorioResponse>> ListarAsync();

        Task<int> CrearAsync(CrearConsultorioRequest request);

    }
}
