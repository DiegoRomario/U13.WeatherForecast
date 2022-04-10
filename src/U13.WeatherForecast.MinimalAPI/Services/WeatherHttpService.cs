using System.Net;
using System.Text.Json;
using U13.WeatherForecast.MinimalAPI.Models.WeatherForecast;
using U13.WeatherForecast.MinimalAPI.Models.GridPoints;
using Microsoft.Extensions.Options;
using U13.WeatherForecast.MinimalAPI.Configs;

namespace U13.WeatherForecast.MinimalAPI.Services
{
    public class WeatherHttpService : IWeatherHttpService
    {
        private readonly HttpClient httpClient;
        private readonly JsonSerializerOptions options;
        private readonly HttpClientSettings httpClientSettings;

        public WeatherHttpService(HttpClient httpClient, IOptions<HttpClientSettings> httpClientSettings)
        {
            this.httpClientSettings = httpClientSettings.Value;
            this.httpClient = httpClient;
            this.httpClient.BaseAddress = new Uri(this.httpClientSettings.WeatherBase);
            this.httpClient.DefaultRequestHeaders.Add("User-Agent", "U13");
            options = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
        }
        public async Task<GridPointsResult> GetGridPointByCoordinates(double x, double y)
        {
            GridPointsResult result = default;
            var response = await httpClient.GetAsync(string.Format(httpClientSettings.GridPointsByCoordinates, x.ToString().Replace(",", "."), y.ToString().Replace(",", ".")));
            response.EnsureSuccessStatusCode();
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var content = await response.Content.ReadAsStringAsync();
                result = JsonSerializer.Deserialize<GridPointsResult>(content, options);
            }
            return result;
        }

        public async Task<WeatherForecastResult> GetWeatherForecastByGrid(string gridId, int gridX, int gridY)
        {
            WeatherForecastResult weatherForecastResult = default;
            var response = await httpClient.GetAsync(string.Format(httpClientSettings.ForecastByGrid, gridId, gridX, gridY));
            response.EnsureSuccessStatusCode();
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var content = await response.Content.ReadAsStringAsync();
                weatherForecastResult = JsonSerializer.Deserialize<WeatherForecastResult>(content, options);
            }
            return weatherForecastResult;
        }
    }
}
