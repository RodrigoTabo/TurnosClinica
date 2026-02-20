namespace TurnosClinica.ApiClients.Medicos
{
    using TurnosClinica.ApiClients.Common;
    using TurnosClinica.Application.DTOs.Medicos;
    using static System.Runtime.InteropServices.JavaScript.JSType;

    public class MedicosApiClient
    {
        private readonly HttpClient _Http;

        public MedicosApiClient(HttpClient http)
        {
            _Http = http;
        }

        public async Task<List<MedicoResponse>> ListarAsync(string nombre)
        {

            var n = (nombre ?? "").Trim();
            var url = string.IsNullOrEmpty(nombre)
                ? "api/medicos" 
                : $"api/medicos?nombre={Uri.EscapeDataString(n)}";

            return await _Http.GetJsonOrThrowAsync<List<MedicoResponse>>(url);
        }

        public async Task<int> CrearAsync(CrearMedicoRequest request)
        {
            var created = await _Http.PostJsonOrThrowAsync<CrearMedicoRequest, CreatedIdResponse>("api/medicos", request);
            return created.Id;
        }

        public async Task<MedicoResponse> GetByIdAsync(int id)
            => await _Http.GetJsonOrThrowAsync<MedicoResponse>($"api/medicos/{id}");

        public async Task UpdateAsync(int id, UpdateMedicoRequest request)
            => await _Http.PutJsonOrThrowAsync($"api/medicos/{id}", request);

        public async Task SoftDeleteAsync(int id)
            => await _Http.DeleteOrThrowAsync($"api/medicos/{id}");

        private class CreatedIdResponse
        {
            public int Id { get; set; }
        }

    }
}
