using TurnosClinica.ApiClients.Common;
using TurnosClinica.Application.DTOs.Ciudades;

namespace TurnosClinica.ApiClients.Ciudades
{
    public class CiudadesApiClient
    {

        private readonly HttpClient _http;

        public CiudadesApiClient(HttpClient http)
        {
            _http = http;
        }

        public async Task<List<CiudadResponse>> ListarAsync(string? Nombre)
        {
            var n = (Nombre ?? "").Trim();
            var url = string.IsNullOrEmpty(n)
                ? "api/ciudades"
                : $"api/ciudades?Nombre={Uri.EscapeDataString(n)}";

           return await _http.GetJsonOrThrowAsync<List<CiudadResponse>>(url);
        }

        public async Task<int> CrearAsync(CrearCiudadRequest request)
        {
            var created = await _http.PostJsonOrThrowAsync<CrearCiudadRequest, CreatedIdResponse>
                ("api/ciudades", request);

            return created.Id;
        }

        public async Task<CiudadResponse> GetByIdAsync(int id)
            => await _http.GetJsonOrThrowAsync<CiudadResponse>($"api/ciudades/{id}");

        public async Task UpdateAsync(int id, UpdateCiudadRequest request)
            => await _http.PutJsonOrThrowAsync($"api/ciudades/{id}", request);

        public async Task SoftDeleteAsync(int id)
            => await _http.DeleteOrThrowAsync($"api/ciudades/{id}");

        private class CreatedIdResponse
        {
            public int Id { get; set; }
        }


    }
}
