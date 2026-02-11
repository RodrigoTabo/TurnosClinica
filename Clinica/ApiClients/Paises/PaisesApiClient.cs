using TurnosClinica.ApiClients.Common;
using TurnosClinica.ApiClients.Turnos;
using TurnosClinica.Application.DTOs.Medicos;
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



        public Task<List<PaisResponse>> ListarAsync()
            => _http.GetJsonOrThrowAsync<List<PaisResponse>>("api/paises");

        public async Task<int> CrearAsync(CrearPaisRequest request)
        {
            var created = await _http.PostJsonOrThrowAsync<CrearPaisRequest, CreatedIdResponse>("api/paises", request);
            return created.Id;
        }

        private class CreatedIdResponse
        {
            public int Id { get; set; }
        }

    }
}
