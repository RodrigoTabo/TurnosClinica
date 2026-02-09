namespace TurnosClinica.ApiClients.Medicos
{
    using System.Net.Http.Json;
    using TurnosClinica.ApiClients.Turnos;
    using TurnosClinica.Application.DTOs.Medicos;

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


        public async Task<int> CrearAsync(CrearMedicoRequest request)
        {
            var resp = await _Http.PostAsJsonAsync("api/medicos", request);


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
