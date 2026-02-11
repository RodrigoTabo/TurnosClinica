using TurnosClinica.ApiClients.Turnos;
using System.Net.Http.Json;

namespace TurnosClinica.ApiClients.Common
{
    public static class HttpClientExtensions
    {
        //HttpClient extensions: “evita duplicar parseo/errores” Grabate esto wachin, lo estas haciendo espectacular.
        public static async Task<T> GetJsonOrThrowAsync<T>(this HttpClient http, string url)
        {
            var resp = await http.GetAsync(url);

            if (resp.IsSuccessStatusCode)
            {
                var data = await resp.Content.ReadFromJsonAsync<T>();
                if (data is null)
                    throw new TurnosApiException("La API devolvió una respuesta vacía.");

                return data;
            }

            // Si no fue success, convertimos el response en TurnosApiException
            throw await TurnosApiException.FromHttpResponse(resp);
        }

        public static async Task<TResp> PostJsonOrThrowAsync<TReq, TResp>(this HttpClient http, string url, TReq body)
        {
            var resp = await http.PostAsJsonAsync(url, body);

            if (resp.IsSuccessStatusCode)
            {
                var data = await resp.Content.ReadFromJsonAsync<TResp>();
                if (data is null)
                    throw new TurnosApiException("La API devolvió una respuesta vacía.");

                return data;
            }

            throw await TurnosApiException.FromHttpResponse(resp);
        }

    }
}
