using TurnosClinica.ApiClients.Common;
using TurnosClinica.Application.DTOs.Medicos;
using TurnosClinica.Application.DTOs.Provincias;
using static System.Net.WebRequestMethods;

namespace TurnosClinica.ApiClients.Provincias
{
    public class ProvinciasApiClient
    {

        private readonly HttpClient _http;

        public ProvinciasApiClient(HttpClient http)
        {
            _http = http;
        }

        public Task<List<ProvinciaResponse>> ListarAsync()
            => _http.GetJsonOrThrowAsync<List<ProvinciaResponse>>("api/provincias");


        public async Task<int> CrearAsync(CrearProvinciaRequest request)
        {
            var created = await _http.PostJsonOrThrowAsync<CrearProvinciaRequest, CreatedIdResponse>(
                "api/provincias", request);
            return created.Id;
        }

        private class CreatedIdResponse
        {
            public int Id { get; set; }
        }

    }
}
