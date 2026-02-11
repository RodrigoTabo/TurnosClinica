using TurnosClinica.ApiClients.Common;
using TurnosClinica.Application.DTOs.Consultorios;

namespace TurnosClinica.ApiClients.Consultorios
{
    public class ConsultoriosApiClient
    {
        private readonly HttpClient _http;

        public ConsultoriosApiClient(HttpClient http)
        {
            _http = http;
        }


        public Task<List<ConsultorioResponse>> ListarAsync()
            => _http.GetJsonOrThrowAsync<List<ConsultorioResponse>>("api/consultorios");

        public async Task<int> CrearAsync(CrearConsultorioRequest request)
        {
            var created = await _http.PostJsonOrThrowAsync<CrearConsultorioRequest, CreatedIdResponse>("api/consultorios", request);
            return created.Id;
        }

        private class CreatedIdResponse
        {
            public int Id { get; set; }
        }

    }
}
