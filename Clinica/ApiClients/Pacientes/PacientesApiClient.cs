using TurnosClinica.ApiClients.Common;
using TurnosClinica.Application.DTOs.Pacientes;

namespace TurnosClinica.ApiClients.Pacientes
{
    public class PacientesApiClient
    {

        private readonly HttpClient _http;

        public PacientesApiClient(HttpClient http)
        {
            _http = http;
        }

        public async Task<List<PacienteResponse>> ListarAsync(string DNI, string Nombre, string Apellido)
        {
            var url = $"api/pacientes?DNI={Uri.EscapeDataString(DNI)}&Nombre={Uri.EscapeDataString(Nombre)}&Apellido={Uri.EscapeDataString(Apellido)}";

            return await _http.GetJsonOrThrowAsync<List<PacienteResponse>>(url);

        }

        public async Task<Guid> CrearAsync(CrearPacienteRequest request)
        {
            var created = await _http.PostJsonOrThrowAsync<CrearPacienteRequest, CreatedIdResponse>("api/pacientes", request);
            return created.Id;
        }

        public async Task<PacienteSelectorItem> GetIdByDniAsync(string? dni)
        {
            if (string.IsNullOrWhiteSpace(dni))
                throw new ArgumentException("DNI vacío.", nameof(dni));

            var url = $"api/pacientes/id/{Uri.EscapeDataString(dni)}";
            return await _http.GetJsonOrThrowAsync<PacienteSelectorItem>(url);
        }

        public async Task<PacienteResponse> GetByIdAsync(Guid id)
            => await _http.GetJsonOrThrowAsync<PacienteResponse>($"api/pacientes/{id}");

        public async Task UpdateAsync(Guid id, UpdatePacienteRequest request)
            => await _http.PutJsonOrThrowAsync($"api/pacientes/{id}", request);

        public async Task SoftDeleteAsync(Guid id)
            => await _http.DeleteOrThrowAsync($"api/pacientes/{id}");



        private class CreatedIdResponse
        {
            public Guid Id { get; set; }
        }

    }
}
