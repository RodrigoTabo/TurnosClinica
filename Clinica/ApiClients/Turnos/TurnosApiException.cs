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
            StatusCode = 0;
        }


        public static async Task<TurnosApiException> FromHttpResponse(HttpResponseMessage resp)
        {
            var status = resp.StatusCode;

            try
            {
                // 1) ValidationProblemDetails (400) -> trae "errors"
                var vpd = await resp.Content.ReadFromJsonAsync<ValidationProblemDetailsLite>();
                if (vpd?.Errors is not null && vpd.Errors.Count > 0)
                {
                    var first = vpd.Errors
                        .SelectMany(kv => kv.Value ?? Array.Empty<string>())
                        .FirstOrDefault();

                    if (!string.IsNullOrWhiteSpace(first))
                        return new TurnosApiException(first, status);
                }

                // 2) ProblemDetails normal -> title / detail
                var pd = await resp.Content.ReadFromJsonAsync<ProblemDetailsLite>();
                var msg = pd?.Title ?? pd?.Detail;

                if (!string.IsNullOrWhiteSpace(msg))
                    return new TurnosApiException(msg, status);
            }
            catch
            {
                // ignore parse errors
            }

            // 3) fallback
            var text = await resp.Content.ReadAsStringAsync();
            var fallback = string.IsNullOrWhiteSpace(text) ? $"HTTP {(int)status}" : text;
            return new TurnosApiException(fallback, status);
        }

        private class ValidationProblemDetailsLite
        {
            public string? Title { get; set; }
            public Dictionary<string, string[]?>? Errors { get; set; }
        }

        private class ProblemDetailsLite
        {
            public string? Title { get; set; }
            public string? Detail { get; set; }
        }
    }

}
