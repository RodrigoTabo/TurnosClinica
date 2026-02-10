using TurnosClinica.ApiClients.Turnos;
using TurnosClinica.Application.DTOs.Paises;
using static System.Net.WebRequestMethods;

namespace TurnosClinica.ApiClients.Paises
{
    public class PaisesApiClient
    {
        private readonly HttpClient _http;

        public PaisesApiClient(HttpClient http)
        {
            _http = http;
        }


        public async Task<List<PaisResponse>> ListarAsync()
        {
            var url = $"api/paises";

            var result = await _http.GetFromJsonAsync<List<PaisResponse>>(url);

            return result ?? new List<PaisResponse>();

        }

        public async Task<int> CrearAsync(CrearPaisRequest request)
        {
            var resp = await _http.PostAsJsonAsync("api/paises", request);

            if (resp.IsSuccessStatusCode)
            {
                var created = await resp.Content.ReadFromJsonAsync<CreatedIdResponse>();
                return created?.Id ?? 0;
            }

            throw await TurnosApiException.FromHttpResponse(resp);
        }

                private class CreatedIdResponse
        {
            public int Id { get; set; }
        }

    }
}
