namespace TurnosClinica.ApiClients.Turnos
{
    using System.Net;
    using System.Net.Http.Json;

    public class TurnosApiException : Exception
    {
        public HttpStatusCode StatusCode { get; }

        public TurnosApiException(string message, HttpStatusCode statusCode) : base(message)
        {
            StatusCode = statusCode;
        }

        public TurnosApiException(string? message) : base(message)
        {
        }

        public static async Task<TurnosApiException> FromHttpResponse(HttpResponseMessage resp)
        {
            try
            {
                var pd = await resp.Content.ReadFromJsonAsync<ProblemDetailsLite>();
                if (pd != null && !string.IsNullOrWhiteSpace(pd.Title))
                    return new TurnosApiException(pd.Title, resp.StatusCode);
            }
            catch { /* ignore */ }

            // fallback
            var text = await resp.Content.ReadAsStringAsync();
            var msg = string.IsNullOrWhiteSpace(text) ? $"HTTP {(int)resp.StatusCode}" : text;
            return new TurnosApiException(msg, resp.StatusCode);
        }

        private class ProblemDetailsLite
        {
            public string? Title { get; set; }
            public int? Status { get; set; }
            public string? Detail { get; set; }
        }
    }

}
