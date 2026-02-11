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

        public Task<List<CiudadResponse>> ListarAsync()
     => _http.GetJsonOrThrowAsync<List<CiudadResponse>>("api/ciudades");

        public async Task<int> CrearAsync(CrearCiudadRequest request)
        {
            var created = await _http.PostJsonOrThrowAsync<CrearCiudadRequest, CreatedIdResponse>
                ("api/ciudades", request);

            return created.Id;
        }
        private class CreatedIdResponse
        {
            public int Id { get; set; }
        }


    }
}
