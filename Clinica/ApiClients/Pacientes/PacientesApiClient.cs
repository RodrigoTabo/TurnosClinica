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

        private class CreatedIdResponse
        {
            public Guid Id { get; set; }
        }

    }
}
