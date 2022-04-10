using U13.WeatherForecast.MinimalAPI.Models.GeoCoding;

namespace U13.WeatherForecast.MinimalAPI.Services
{
    public interface IGeoCodingHttpService
    {
        Task<GeoCodingResult> GetGeoCodingByAddress(string address);
    }
}