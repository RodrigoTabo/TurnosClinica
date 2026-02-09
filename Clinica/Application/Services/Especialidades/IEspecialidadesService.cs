using TurnosClinica.Application.DTOs.Especialidades;

namespace TurnosClinica.Application.Services.Especialidades
{
    public interface IEspecialidadesService
    {

        Task<List<EspecialidadResponse>> ListarAsync();
        Task<int> CreateAsync(CrearEspecialidadRequest request);


    }
}
