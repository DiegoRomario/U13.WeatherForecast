using Microsoft.Extensions.Logging;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using U13.WeatherForecast.MinimalAPI.Models;
using U13.WeatherForecast.MinimalAPI.Models.GeoCoding;
using U13.WeatherForecast.MinimalAPI.Services;
using Xunit;

namespace U13.WeatherForecast.UnitTests
{
    public class WeatherForecastServiceTests
    {
        const string ANY_ADDRESS = "ANY";
        private readonly string notification2AddressMatches;
        private readonly GeoCodingResult geoCodingResultFake;
        private readonly IWeatherForecastService weatherForecastService;
        private readonly Mock<IGeoCodingHttpService> geoCodingHttpServiceMock;
        private readonly Mock<IWeatherHttpService> weatherHttpServiceMocK;
        private readonly Mock<INotificationService> notificationServiceMock;
        private readonly Mock<ILogger<WeatherForecastService>> loggerMock;


        public WeatherForecastServiceTests()
        {
            notification2AddressMatches = "More than one Geo Coding reference to this address was found";
            geoCodingResultFake = new GeoCodingResult { Result = new Result() };
            geoCodingResultFake.Result.AddressMatches = new List<AddressMatch>() { new AddressMatch(), new AddressMatch() };
            notificationServiceMock = new Mock<INotificationService>();
            geoCodingHttpServiceMock = new Mock<IGeoCodingHttpService>();
            weatherHttpServiceMocK = new Mock<IWeatherHttpService>();
            loggerMock = new Mock<ILogger<WeatherForecastService>>();
            geoCodingHttpServiceMock.Setup(x => x.GetGeoCodingByAddress(It.IsAny<string>())).Returns(Task.FromResult(geoCodingResultFake));
            weatherForecastService = new WeatherForecastService(geoCodingHttpServiceMock.Object, weatherHttpServiceMocK.Object, notificationServiceMock.Object, loggerMock.Object);
        }

        [Fact(DisplayName = @"Given that GetWeatherForecastFor7DaysByAddress method is called,
                              When GetGeoCodingByAddress returns more than one address reference
                              The result Should be null and a notification should be raised with an specific message")]
        [Trait("Category", "Unit")]
        public async void GetGeoCodingByAddress_MoreThanOneAddressReference()
        {
            // Arrange & Act
            var result = await weatherForecastService.GetWeatherForecastFor7DaysByAddress(ANY_ADDRESS);
            // Assert
            notificationServiceMock.Verify(d => d.AddNotification(It.Is<Notification>(x => x.Message == notification2AddressMatches)), Times.Once);
            Assert.Null(result);
        }
    }
}