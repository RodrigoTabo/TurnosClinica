using TurnosClinica.Application.DTOs.Pacientes;

namespace TurnosClinica.Application.Services.Pacientes
{
    public interface IPacientesService
    {

        Task<List<PacienteResponse>> ListarAsync(string? DNI, string? Nombre, string? Apellido);
        Task<Guid> CrearAsync(CrearPacienteRequest request);
        Task<PacienteSelectorItem> GetByDniAsync(string dni);
        Task<PacienteResponse> GetByIdAsync(Guid id);
        Task UpdateAsync(Guid id, UpdatePacienteRequest request);
        Task SoftDeleteAsync(Guid id);

    }
}
