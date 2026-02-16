using TurnosClinica.Application.DTOs.Panel;
using TurnosClinica.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;


namespace TurnosClinica.Application.Services.Panel
{
    public sealed class DashboardService : IDashboardService
    {
        private readonly TurnosDbContext _context;

        public DashboardService(TurnosDbContext context)
        {
            _context = context;
        }

        public async Task<DashboardSummary> GetSummaryAsync()
        {
            var today = DateOnly.FromDateTime(DateTime.Today);

            var medicosActivos = await _context.Medicos.CountAsync(m => m.Activo);
            var nuevosPacientes = await _context.Pacientes.CountAsync(p => p.FechaAlta == today);
            var turnosHoy = await _context.Turnos.CountAsync(t => t.Fecha == today);
            var consultorios = await _context.Consultorios.CountAsync();

            return new DashboardSummary(medicosActivos, nuevosPacientes, turnosHoy, consultorios);
        }
    }

}
