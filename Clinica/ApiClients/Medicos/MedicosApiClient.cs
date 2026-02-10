namespace TurnosClinica.ApiClients.Medicos
{
    using System.Net.Http.Json;
    using TurnosClinica.ApiClients.Common;
    using TurnosClinica.ApiClients.Turnos;
    using TurnosClinica.Application.DTOs.Medicos;

    public class MedicosApiClient
    {
        private readonly HttpClient _Http;

        public MedicosApiClient(HttpClient http)
        {
            _Http = http;
        }

        public Task<List<MedicoResponse>> ListarAsync()
        => _Http.GetJsonOrThrowAsync<List<MedicoResponse>>("api/medicos");



        public async Task<int> CrearAsync(CrearMedicoRequest request)
        {
            var created = await _Http.PostJsonOrThrowAsync<CrearMedicoRequest, CreatedIdResponse>(
                "api/medicos", request);

            return created.Id;
        }

        private class CreatedIdResponse
        {
            public int Id { get; set; }
        }

    }
}
