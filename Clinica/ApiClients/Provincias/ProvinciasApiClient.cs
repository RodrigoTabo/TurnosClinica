using TurnosClinica.ApiClients.Common;
using TurnosClinica.Application.DTOs.Medicos;
using TurnosClinica.Application.DTOs.Provincias;
using static System.Net.WebRequestMethods;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TurnosClinica.ApiClients.Provincias
{
    public class ProvinciasApiClient
    {

        private readonly HttpClient _http;

        public ProvinciasApiClient(HttpClient http)
        {
            _http = http;
        }

        public async Task<List<ProvinciaResponse>> ListarAsync(string? Nombre)
        {

            var n = (Nombre ?? "").Trim();
            var url = string.IsNullOrEmpty(n) ? "api/provincias" : $"api/provincias?nombre={Uri.EscapeDataString(n)}";

            return await _http.GetJsonOrThrowAsync<List<ProvinciaResponse>>(url);
        }


        public async Task<int> CrearAsync(CrearProvinciaRequest request)
        {
            var created = await _http.PostJsonOrThrowAsync<CrearProvinciaRequest, CreatedIdResponse>(
                "api/provincias", request);
            return created.Id;
        }

        public async Task<ProvinciaResponse> GetByIdAsync(int id)
        {
            return await _http.GetJsonOrThrowAsync<ProvinciaResponse>($"api/provincias/{id}");
        }

        public async Task UpdateAsync(int id, UpdateProvinciaRequest request)
        {
            await _http.PutJsonOrThrowAsync($"api/provincias/{id}", request);
        }

        public async Task SoftDeleteAsync(int id)
        {
            await _http.DeleteOrThrowAsync($"api/provincias/{id}");
        }

        private class CreatedIdResponse
        {
            public int Id { get; set; }
        }

    }
}
