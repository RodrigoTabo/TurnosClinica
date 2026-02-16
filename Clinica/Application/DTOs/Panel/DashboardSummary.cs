namespace TurnosClinica.Application.DTOs.Panel
{
    public sealed record DashboardSummary(
        int MedicosActivos,
        int NuevosPacientes,
        int TurnosHoy,
        int Consultorios
    );

}
