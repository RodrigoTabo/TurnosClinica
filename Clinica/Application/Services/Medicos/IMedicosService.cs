using TurnosClinica.Application.DTOs.Medicos;

namespace TurnosClinica.Application.Services.Medicos
{
    public interface IMedicosService
    {

        Task<int> CrearAsync(CrearMedicoRequest request);
        Task<List<MedicoResponse>> ListarAsync(string nombre);

        Task<MedicoResponse> GetByIdAsync(int id);

        Task UpdateAsync(int id, UpdateMedicoRequest request);

        Task SoftDeleteAsync(int id);

    }
}
