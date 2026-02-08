namespace TurnosClinica.ApiClients
{
    using System.Net;
    using System.Net.Http.Json;
    using TurnosClinica.Application.DTOs;

    public class TurnosApiClient
    {
        private readonly HttpClient _http;

        public TurnosApiClient(HttpClient http)
        {
            _http = http;
        }

        public async Task<List<TurnoResponse>> ListarAsync(DateTime desde, DateTime hasta, int? medicoId = null)
        {
            var url = $"api/turnos?desde={Uri.EscapeDataString(desde.ToString("O"))}&hasta={Uri.EscapeDataString(hasta.ToString("O"))}";
            if (medicoId.HasValue)
                url += $"&medicoId={medicoId.Value}";

            var result = await _http.GetFromJsonAsync<List<TurnoResponse>>(url);
            return result ?? new List<TurnoResponse>();
        }

        public async Task<int> CrearAsync(CrearTurnoRequest request)
        {
            var resp = await _http.PostAsJsonAsync("api/turnos", request);

            if (resp.IsSuccessStatusCode)
            {
                // Si la API devuelve { id: 123 } lo leemos.
                // Si la API devuelve vacío, igual lo manejamos luego.
                var created = await resp.Content.ReadFromJsonAsync<CreatedIdResponse>();
                return created?.Id ?? 0;
            }

            throw await TurnosApiException.FromHttpResponse(resp);
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
