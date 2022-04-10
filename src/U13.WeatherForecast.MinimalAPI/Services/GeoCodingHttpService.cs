using Microsoft.Extensions.Options;
using System.Net;
using System.Text.Json;
using U13.WeatherForecast.MinimalAPI.Configs;
using U13.WeatherForecast.MinimalAPI.Models.GeoCoding;

namespace U13.WeatherForecast.MinimalAPI.Services
{
    public class GeoCodingHttpService : IGeoCodingHttpService
    {
        private readonly HttpClient httpClient;
        private readonly JsonSerializerOptions options;
        private readonly HttpClientSettings httpClientSettings;

        public GeoCodingHttpService(HttpClient httpClient, IOptions<HttpClientSettings> httpClientSettings)
        {
            this.httpClientSettings = httpClientSettings.Value;
            this.httpClient = httpClient;
            this.httpClient.BaseAddress = new Uri(this.httpClientSettings.GeocodingBase);
            this.httpClient.DefaultRequestHeaders.Add("User-Agent", "U13");
            options = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
        }

        public async Task<GeoCodingResult> GetGeoCodingByAddress(string address)
        {
            GeoCodingResult result = default;
            var response = await httpClient.GetAsync(string.Format(httpClientSettings.GeocodingByAddress, address));
            response.EnsureSuccessStatusCode();
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var content = await response.Content.ReadAsStringAsync();
                result = JsonSerializer.Deserialize<GeoCodingResult>(content, options);
            }
            return result;
        }

    }
}
