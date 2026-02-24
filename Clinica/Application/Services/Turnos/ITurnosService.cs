using TurnosClinica.Application.DTOs.Turnos;

namespace TurnosClinica.Application.Services.Turnos
{
    public interface ITurnosService
    {

        Task<int> CrearAsync(CrearTurnoRequest request);
        Task<List<TurnoResponse>> ListarAsync(DateTime desde, DateTime hasta, int? medicoId = null);
        Task CambiarEstadoAsync(int turnoId, CambiarEstadoRequest request);

        Task<TurnoResponse> GetByIdAsync(int id);
        Task UpdateAsync(int Id, UpdateTurnoRequest request);

        Task SoftDeleteAsync(int Id);

        Task<Guid> ObtenerOCrearPacientePorDniAsync(CrearTurnoConPacienteRequest request);

        Task<int> CrearAsegurandoPacienteAsync(CrearTurnoConPacienteRequest request);

        Task<(bool Ok, string Mensaje)> VerificarTurnoAsync(int turnoId, string token);
        Task<List<TimeOnly>> ObtenerHorariosDisponiblesAsync(int medicoId, int consultorioId, DateOnly fecha);



    }
}
