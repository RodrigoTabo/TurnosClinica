namespace TurnosClinica.ApiClients.Turnos
{
    using System.Net.Http.Json;
    using TurnosClinica.ApiClients.Common;
    using TurnosClinica.Application.DTOs.Turnos;

    public class TurnosApiClient
    {
        private readonly HttpClient _http;

        public TurnosApiClient(HttpClient http)
        {
            _http = http;
        }

        public Task<List<TurnoResponse>> ListarAsync(DateTime desde, DateTime hasta, int? medicoId = null)
        {
            var url = $"api/turnos?desde={Uri.EscapeDataString(desde.ToString("O"))}&hasta={Uri.EscapeDataString(hasta.ToString("O"))}";

            if (medicoId is not null)
                url += $"&medicoId={medicoId.Value}";

            return _http.GetJsonOrThrowAsync<List<TurnoResponse>>(url);
        }

        public async Task<int> CrearAsync(CrearTurnoRequest request)
        {
            var created = await _http.PostJsonOrThrowAsync<CrearTurnoRequest, CreatedIdResponse>("api/turnos", request);
            return created.Id;
        }

        public async Task CambiarEstadoAsync(int turnoId, CambiarEstadoRequest request)
        {
            var resp = await _http.PatchAsJsonAsync($"api/turnos/{turnoId}/estado", request);

            if (resp.IsSuccessStatusCode) return;

            throw await TurnosApiException.FromHttpResponse(resp);
        }

        private class CreatedIdResponse
        {
            public int Id { get; set; }
        }
    }

}
