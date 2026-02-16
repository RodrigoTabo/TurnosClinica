using TurnosClinica.Application.DTOs.Pacientes;

namespace TurnosClinica.Application.Services.Pacientes
{
    public interface IPacientesService
    {

        Task<List<PacienteResponse>> ListarAsync(string? DNI, string? Nombre, string? Apellido);
        Task<Guid> CrearAsync(CrearPacienteRequest request);

    }
}
