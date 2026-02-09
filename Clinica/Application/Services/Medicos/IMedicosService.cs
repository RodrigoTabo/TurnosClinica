using TurnosClinica.Application.DTOs.Medicos;

namespace TurnosClinica.Application.Services.Medicos
{
    public interface IMedicosService
    {

        Task<int> CrearAsync(CrearMedicoRequest request);
        Task<List<MedicoResponse>> ListarAsync();
    }
}
