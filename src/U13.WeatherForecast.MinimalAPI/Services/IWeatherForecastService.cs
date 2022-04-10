using U13.WeatherForecast.MinimalAPI.Models.WeatherForecast;

namespace U13.WeatherForecast.MinimalAPI.Services
{
    public interface IWeatherForecastService
    {
        Task<IEnumerable<Period>> GetWeatherForecastForTheNext7DaysByAddress(string address);
    }
}
