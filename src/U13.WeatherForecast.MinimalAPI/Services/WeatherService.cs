using System.Net;
using System.Text.Json;
using U13.WeatherForecast.MinimalAPI.Models.WeatherForecast;
using U13.WeatherForecast.MinimalAPI.Models.GridPoints;

namespace U13.WeatherForecast.MinimalAPI.Services
{
    public class WeatherService : IWeatherService
    {
        const string GET_GRID_POINTS_BY_COORDINATES_URL = "https://api.weather.gov/points/{1},{0}";
        const string GET_FORECAST_BY_GRID = "https://api.weather.gov/gridpoints/{0}/{1},{2}/forecast";
        private readonly HttpClient httpClient;
        public WeatherService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<GridPointsResult> GetGridPointByCoordinates(double x, double y)
        {
            GridPointsResult result = default;
            var options = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
            httpClient.DefaultRequestHeaders.Add("User-Agent", "U13");
            var response = await httpClient.GetAsync(string.Format(GET_GRID_POINTS_BY_COORDINATES_URL, x.ToString().Replace(",", "."), y.ToString().Replace(",", ".")));
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
            var options = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
            httpClient.DefaultRequestHeaders.Add("User-Agent", "U13");
            var response = await httpClient.GetAsync(string.Format(GET_FORECAST_BY_GRID, gridId, gridX, gridY));
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var content = await response.Content.ReadAsStringAsync();
                weatherForecastResult = JsonSerializer.Deserialize<WeatherForecastResult>(content, options);
            }
            return weatherForecastResult;
        }
    }
}
