using U13.WeatherForecast.MinimalAPI.Models.GridPoints;
using U13.WeatherForecast.MinimalAPI.Models.WeatherForecast;

namespace U13.WeatherForecast.MinimalAPI.Services
{
    public interface IWeatherHttpService
    {
        Task<GridPointsResult> GetGridPointByCoordinates(double latitude, double longitude);
        Task<WeatherForecastResult> GetWeatherForecastByGrid(string gridId, int gridX, int gridY);
    }
}