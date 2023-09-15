using System.Text.Json;

namespace HotelApp.Client.Servicios
{
    public class HttpServicio : IHttpServicio
    {
        private readonly HttpClient http;

        public HttpServicio(HttpClient http)
        {
            this.http = http;
        }

        public async Task<HttpRespuesta<T>> Get<T>(string url)
        {
            var response = await http.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var respuesta = await DesSerlizar<T>(response);
                return new HttpRespuesta<T>(respuesta, false, response);
            }
            else
            {
                return new HttpRespuesta<T>(default, true, response);
            }

        }

        private async Task<T?> DesSerlizar<T>(HttpResponseMessage response)
        {
            var respuestaStr = await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<T>(respuestaStr,
                new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
        }
    }
}
