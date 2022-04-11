using U13.WeatherForecast.MinimalAPI.Models;
using U13.WeatherForecast.MinimalAPI.Models.GeoCoding;
using U13.WeatherForecast.MinimalAPI.Models.GridPoints;
using U13.WeatherForecast.MinimalAPI.Models.WeatherForecast;

namespace U13.WeatherForecast.MinimalAPI.Services
{
    public class WeatherForecastService : IWeatherForecastService
    {
        private readonly IGeoCodingHttpService geoCodingHttpService;
        private readonly IWeatherHttpService weatherHttpService;
        private readonly INotificationService notificationService;
        private readonly ILogger logger;

        public WeatherForecastService(IGeoCodingHttpService geoCodingHttpService, IWeatherHttpService weatherHttpService, INotificationService notificationService, ILogger<WeatherForecastService> logger)
        {
            this.geoCodingHttpService = geoCodingHttpService;
            this.weatherHttpService = weatherHttpService;
            this.notificationService = notificationService;
            this.logger = logger;
        }
        public async Task<IEnumerable<Period>> GetWeatherForecastFor7DaysByAddress(string address)
        {
            try
            {
                IEnumerable<Period> WeatherForecastFor7Days = default;
                Coordinates coodinates = await GetCoordinates(address);
                GridPointsResult gridPoints = await GetGridPointByCoordinates(coodinates);
                WeatherForecastResult weatherForecast = await GetWeatherForecastByGrid(gridPoints);

                if (notificationService.HasNotification()) return WeatherForecastFor7Days;

                if (weatherForecast is not null)
                    WeatherForecastFor7Days = weatherForecast.Properties.Periods;
                else
                    await notificationService.AddNotification(new Notification("Weather Forecast not found"));

                return WeatherForecastFor7Days;
            }
            catch (Exception exception)
            {
                logger.LogError(exception, $"An error occurred while we are getting the weather forecast");
                throw;
            }
        }
        private async Task<Coordinates> GetCoordinates(string address)
        {
            Coordinates coodinates = default;

            if (notificationService.HasNotification()) return coodinates;

            GeoCodingResult geoCoding = await geoCodingHttpService.GetGeoCodingByAddress(address);
            if (geoCoding is null)
                await notificationService.AddNotification(new Notification("Geo Coding not found"));
            else if (geoCoding.Result.AddressMatches.Count > 1)
                await notificationService.AddNotification(new Notification("More than one Geo Coding reference to this address was found"));
            else if (geoCoding.Result.AddressMatches.Count == 1)
                coodinates = geoCoding.Result.AddressMatches.FirstOrDefault().Coordinates;
            return coodinates;
        }
        private async Task<GridPointsResult> GetGridPointByCoordinates(Coordinates coodinates)
        {
            GridPointsResult gridPoints = default;

            if (notificationService.HasNotification()) return gridPoints;

            if (coodinates is not null)
                gridPoints = await weatherHttpService.GetGridPointByCoordinates(coodinates.X, coodinates.Y);
            else
                await notificationService.AddNotification(new Notification("Coordinates not found"));

            return gridPoints;

        }
        private async Task<WeatherForecastResult> GetWeatherForecastByGrid(GridPointsResult gridPoints)
        {
            WeatherForecastResult weatherForecastResult = default;

            if (notificationService.HasNotification()) return weatherForecastResult;

            if (gridPoints?.Properties is not null)
                weatherForecastResult = await weatherHttpService.GetWeatherForecastByGrid(gridPoints.Properties.GridId, gridPoints.Properties.GridX, gridPoints.Properties.GridY);
            else
                await notificationService.AddNotification(new Notification("Grid Points not found"));

            return weatherForecastResult;
        }


    }
}
