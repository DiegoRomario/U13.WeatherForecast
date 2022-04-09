using U13.WeatherForecast.MinimalAPI.Models.GeoCoding;

namespace U13.WeatherForecast.MinimalAPI.Services
{
    public interface IGeoCodingService
    {
        Task<GeoCodingResult> GetGeoCodingByAddress(string address);
    }
}