namespace TurnosClinica.ApiClients.Especialidades
{
    using System.Net.Http.Json;
    using TurnosClinica.ApiClients.Common;
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
            var created = _Http.PostJsonOrThrowAsync<CrearEspecialidadRequest, CreatedIdResponse>("api/especialidades", request);
            return created.Id;
        }
        private class CreatedIdResponse
        {
            public int Id { get; set; }
        }

    }
}
