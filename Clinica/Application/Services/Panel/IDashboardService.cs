using TurnosClinica.Application.DTOs.Panel;

namespace TurnosClinica.Application.Services.Panel
{
    public interface IDashboardService
    {
        Task<DashboardSummary> GetSummaryAsync();
    }
}
