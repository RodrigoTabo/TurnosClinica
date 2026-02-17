using TurnosClinica.ApiClients.Common;
using TurnosClinica.Application.DTOs.Paises;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TurnosClinica.ApiClients.Paises
{
    public class PaisesApiClient
    {
        private readonly HttpClient _http;

        public PaisesApiClient(HttpClient http)
        {
            _http = http;
        }

        public async Task<List<PaisResponse>> ListarAsync(string? Nombre)
        {
            var n = (Nombre ?? "").Trim();
            var url = string.IsNullOrEmpty(n)
                ? "api/paises"
                : $"api/paises?Nombre={Uri.EscapeDataString(n)}";

            return await _http.GetJsonOrThrowAsync<List<PaisResponse>>(url);
        }

        public async Task<int> CrearAsync(CrearPaisRequest request)
        {
            var created = await _http.PostJsonOrThrowAsync<CrearPaisRequest, CreatedIdResponse>("api/paises", request);
            return created.Id;
        }

        public Task<PaisResponse> GetByIdAsync(int id)
            => _http.GetJsonOrThrowAsync<PaisResponse>($"api/paises/{id}");

        public Task UpdateAsync(int id, UpdatePaisRequest request)
            => _http.PutJsonOrThrowAsync($"api/paises/{id}", request);

        public Task SoftDeleteAsync(int id)
            => _http.DeleteOrThrowAsync($"api/paises/{id}");

        private class CreatedIdResponse
        {
            public int Id { get; set; }
        }

    }
}
