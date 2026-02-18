using TurnosClinica.ApiClients.Common;
using TurnosClinica.Application.DTOs.Estados;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TurnosClinica.ApiClients.Estados
{
    public class EstadosApiClient
    {
        private readonly HttpClient _http;

        public EstadosApiClient(HttpClient http)
        {
            _http = http;
        }

        public async Task<List<EstadoResponse>> ListarAsync(string? nombre)
        {
            var n = (nombre ?? "").Trim();

            var url = string.IsNullOrEmpty(n)
                ? "api/estados"
                : $"api/estados?nombre={Uri.EscapeDataString(n)}";

            return await _http.GetJsonOrThrowAsync<List<EstadoResponse>>(url);
        }

        public async Task<int> CrearAsync(CrearEstadoRequest request)
        {
            var created = _http.PostJsonOrThrowAsync<CrearEstadoRequest, CreatedIdResponse>("api/estados", request);
            return created.Id;
        }

        public async Task<EstadoResponse> GetByIdAsync(int id)
            => await _http.GetJsonOrThrowAsync<EstadoResponse>($"api/estados/{id}");

        public async Task UpdateAsync(int id, UpdateEstadoRequest request)
            => await _http.PutJsonOrThrowAsync($"api/estados/{id}", request);

        public async Task SoftDeleteAsync(int id)
            => await _http.DeleteOrThrowAsync($"api/estados/{id}");

        private class CreatedIdResponse
        {
            public int Id { get; set; }
        }

    }

}
