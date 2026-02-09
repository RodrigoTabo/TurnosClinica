namespace TurnosClinica.ApiClients.Especialidades
{
    using System.Net.Http.Json;
    using TurnosClinica.ApiClients.Turnos;
    using TurnosClinica.Application.DTOs.Especialidades;
    using static System.Net.WebRequestMethods;

    public class EspecialidadesApiClient
    {
        private readonly HttpClient _Http;

        public EspecialidadesApiClient(HttpClient http)
        {
            _Http = http;
        }

        public async Task<List<EspecialidadResponse>> ListarAsync()
        {
            var url = $"api/especialidades";

            var result = await _Http.GetFromJsonAsync<List<EspecialidadResponse>>(url);
            return result ?? new List<EspecialidadResponse>();
        }

        public async Task<int> CrearAsync(CrearEspecialidadRequest request)
        {
            var resp = await _Http.PostAsJsonAsync("api/especialidades", request);

            if (resp.IsSuccessStatusCode)
            {
                // Si la API devuelve { id: 123 } lo leemos.
                // Si la API devuelve vacío, igual lo manejamos luego.
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
