using TurnosClinica.ApiClients.Common;
using TurnosClinica.Application.DTOs.Consultorios;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TurnosClinica.ApiClients.Consultorios
{
    public class ConsultoriosApiClient
    {
        private readonly HttpClient _http;

        public ConsultoriosApiClient(HttpClient http)
        {
            _http = http;
        }


        public async Task<List<ConsultorioResponse>> ListarAsync(string? nombre)
        {
            var n = (nombre ?? "").Trim();
            var url = string.IsNullOrEmpty(n) ? "api/consultorios" : $"api/consultorios?nombre={Uri.EscapeDataString(n)}";

            return await _http.GetJsonOrThrowAsync<List<ConsultorioResponse>>(url);
        }

        public async Task<int> CrearAsync(CrearConsultorioRequest request)
        {
            var created = await _http.PostJsonOrThrowAsync<CrearConsultorioRequest, CreatedIdResponse>("api/consultorios", request);
            return created.Id;
        }

        public async Task<ConsultorioResponse> GetByIdAsync(int id)
            => await _http.GetJsonOrThrowAsync<ConsultorioResponse>($"api/consultorios/{id}");

        public async Task UpdateAsync(int id, UpdateConsultorioRequest request)
            => await _http.PutJsonOrThrowAsync($"api/consultorios/{id}", request);

        public async Task SoftDeleteAsync(int id)
            => await _http.DeleteAsync($"api/consultorios/{id}");

        private class CreatedIdResponse
        {
            public int Id { get; set; }
        }

    }
}
