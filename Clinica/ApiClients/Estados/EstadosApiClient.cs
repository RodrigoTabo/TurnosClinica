using TurnosClinica.ApiClients.Common;
using TurnosClinica.Application.DTOs.Estados;

namespace TurnosClinica.ApiClients.Estados
{
    public class EstadosApiClient
    {
        private readonly HttpClient _http;

        public EstadosApiClient(HttpClient http)
        {
            _http = http;
        }

        public async Task<List<EstadoResponse>> ListarAsync()
        {
            var url = $"api/estados";

            var result = await _http.GetFromJsonAsync<List<EstadoResponse>>(url);
            return result ?? new List<EstadoResponse>();
        }

        public async Task<int> CrearAsync(CrearEstadoRequest request)
        {
            var created = _http.PostJsonOrThrowAsync<CrearEstadoRequest, CreatedIdResponse>("api/estados", request);
            return created.Id;
        }
        private class CreatedIdResponse
        {
            public int Id { get; set; }
        }

    }

}
