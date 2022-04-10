using U13.WeatherForecast.MinimalAPI.Models;
using U13.WeatherForecast.MinimalAPI.Models.GeoCoding;
using U13.WeatherForecast.MinimalAPI.Models.GridPoints;
using U13.WeatherForecast.MinimalAPI.Models.WeatherForecast;

namespace U13.WeatherForecast.MinimalAPI.Services
{
    public class WeatherForecastService : IWeatherForecastService
    {
        private const int CURRENT_PERIOD_AND_NEXT_7_DAYS = 8;
        private readonly IGeoCodingHttpService geoCodingService;
        private readonly IWeatherHttpService weatherService;
        private readonly INotificationService notificationHandlerService;
        private readonly ILogger logger;

        public WeatherForecastService(IGeoCodingHttpService geoCodingService, IWeatherHttpService weatherService, INotificationService notificationHandlerService, ILogger<WeatherForecastService> logger)
        {
            this.geoCodingService = geoCodingService;
            this.weatherService = weatherService;
            this.notificationHandlerService = notificationHandlerService;
            this.logger = logger;
        }
        public async Task<IEnumerable<Period>> GetWeatherForecastForTheNext7DaysByAddress(string address)
        {
            try
            {
                IEnumerable<Period> WeatherForecastForNowAndNext7Days = default;
                Coordinates coodinates = await GetCoordinates(address);
                GridPointsResult gridPoints = await GetGridPointByCoordinates(coodinates);
                WeatherForecastResult weatherForecast = await GetWeatherForecastByGrid(gridPoints);

                if (notificationHandlerService.HasNotification()) return WeatherForecastForNowAndNext7Days;

                if (weatherForecast is not null)
                    WeatherForecastForNowAndNext7Days = weatherForecast.Properties.Periods.Take(CURRENT_PERIOD_AND_NEXT_7_DAYS);
                else
                    await notificationHandlerService.AddNotification(new Notification("Weather Forecast not found"));

                return WeatherForecastForNowAndNext7Days;
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

            if (notificationHandlerService.HasNotification()) return coodinates;

            GeoCodingResult geoCoding = await geoCodingService.GetGeoCodingByAddress(address);
            if (geoCoding is null)
                await notificationHandlerService.AddNotification(new Notification("Geo Coding not found"));
            else if (geoCoding.Result.AddressMatches.Count > 1)
                await notificationHandlerService.AddNotification(new Notification("More than one Geo Coding reference to this address was found"));
            else if (geoCoding.Result.AddressMatches.Count == 1)
                coodinates = geoCoding.Result.AddressMatches.FirstOrDefault().Coordinates;
            return coodinates;
        }
        private async Task<GridPointsResult> GetGridPointByCoordinates(Coordinates coodinates)
        {
            GridPointsResult gridPoints = default;

            if (notificationHandlerService.HasNotification()) return gridPoints;

            if (coodinates is not null)
                gridPoints = await weatherService.GetGridPointByCoordinates(coodinates.X, coodinates.Y);
            else
                await notificationHandlerService.AddNotification(new Notification("Coordinates not found"));

            return gridPoints;

        }
        private async Task<WeatherForecastResult> GetWeatherForecastByGrid(GridPointsResult gridPoints)
        {
            WeatherForecastResult weatherForecastResult = default;

            if (notificationHandlerService.HasNotification()) return weatherForecastResult;

            if (gridPoints?.Properties is not null)
                weatherForecastResult = await weatherService.GetWeatherForecastByGrid(gridPoints.Properties.GridId, gridPoints.Properties.GridX, gridPoints.Properties.GridY);
            else
                await notificationHandlerService.AddNotification(new Notification("Grid Points not found"));

            return weatherForecastResult;
        }


    }
}
