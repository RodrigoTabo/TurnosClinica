namespace TurnosClinica.ApiClients.Medicos
{
    using TurnosClinica.Application.DTOs.Medicos;
    using System.Net.Http.Json;
    public class MedicosApiClient
    {
        private readonly HttpClient _Http;

        public MedicosApiClient(HttpClient http)
        {
            _Http = http;
        }

        public async Task<List<MedicoResponse>> ListarAsync()
        {
            var url = $"api/medicos";

            var result = await _Http.GetFromJsonAsync<List<MedicoResponse>>(url);
            return result ?? new List<MedicoResponse>();
        }


    }
}
