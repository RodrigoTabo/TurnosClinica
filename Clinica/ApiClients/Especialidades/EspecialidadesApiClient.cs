namespace TurnosClinica.ApiClients.Especialidades
{
    using System.Net.Http.Json;
    using TurnosClinica.ApiClients.Common;
    using TurnosClinica.Application.DTOs.Ciudades;
    using TurnosClinica.Application.DTOs.Especialidades;
    using static System.Net.WebRequestMethods;

    public class EspecialidadesApiClient
    {
        private readonly HttpClient _Http;

        public EspecialidadesApiClient(HttpClient http)
        {
            _Http = http;
        }

        public async Task<List<EspecialidadResponse>> ListarAsync(string? nombre)
        {
            var n = (nombre ?? "").Trim();
            var url = string.IsNullOrEmpty(n) ? $"api/especialidades" : $"api/especialidades?nombre={Uri.EscapeDataString(n)}";

            return await _Http.GetJsonOrThrowAsync<List<EspecialidadResponse>>(url);

        }

        public async Task<int> CrearAsync(CrearEspecialidadRequest request)
        {
            var created = _Http.PostJsonOrThrowAsync<CrearEspecialidadRequest, CreatedIdResponse>("api/especialidades", request);
            return created.Id;
        }

        public async Task<EspecialidadResponse> GetByIdAsync(int id)
            => await _Http.GetJsonOrThrowAsync<EspecialidadResponse>($"api/especialidades/{id}");

        public async Task UpdateAsync(int id, UpdateEspecialidadesRequest request)
            => await _Http.PutJsonOrThrowAsync($"api/especialidades/{id}", request);

        public async Task SoftDeleteAsync(int id)
            => await _Http.DeleteOrThrowAsync($"api/especialidades/{id}");

        private class CreatedIdResponse
        {
            public int Id { get; set; }
        }

    }
}
