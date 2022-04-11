using Microsoft.AspNetCore.Mvc.Testing;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using U13.WeatherForecast.MinimalAPI.Models;
using U13.WeatherForecast.MinimalAPI.Models.WeatherForecast;
using Xunit;

namespace U13.WeatherForecast.IntegrationTests
{
    public class GetWeatherForecastByAddressTests
    {
        const string GET_WEATHER_FORECAST_BY_ADDRESS_PATH = "/getWeatherForecastByAddress?Address=";
        const int PERIODS = 14;

        [Theory(DisplayName = $@"Given that GetWeatherForecastByAddress end-point is called
                                When it receive a valid address, 
                                it Should return status code 200 and an IEnumerable of 14 Periods")]
        [InlineData("4600 SILVER HILL RD, WASHINGTON, DC, 20233")]
        [InlineData("5323 W CENTINELA AVE, LOS ANGELES, CA, 90045")]
        [InlineData("252 BROADWAY, SAN DIEGO, CA, 92101")]
        [Trait("Category", "Integration")]
        public async Task GetWeatherForecastByAddress_ValidAdresses(string address)
        {
            // Arrange
            await using var application = new WebApplicationFactory<Program>();
            HttpClient client = application.CreateClient();
            // Act
            var response = await client.GetAsync($"{GET_WEATHER_FORECAST_BY_ADDRESS_PATH}{address}");
            var json = await response.Content.ReadAsStringAsync();
            var weatherForecastList = JsonSerializer.Deserialize<IEnumerable<Period>>(json);
            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(PERIODS, weatherForecastList.Count());
        }

        [Theory(DisplayName = @"Given that GetWeatherForecastByAddress end-point is called
                                When it receive an invalid address, 
                                it Should return status code 404 and a List of notifications")]
        [InlineData("INVALID ADDRESS 1")]
        [InlineData("123 TEST, ANY, ABC, 321")]
        [Trait("Category", "Integration")]
        public async Task GetWeatherForecastByAddress_InValidAdresses(string address)
        {
            // Arrange
            await using var application = new WebApplicationFactory<Program>();
            HttpClient client = application.CreateClient();
            // Act
            var response = await client.GetAsync($"{GET_WEATHER_FORECAST_BY_ADDRESS_PATH}{address}");
            var json = await response.Content.ReadAsStringAsync();
            var notifications = JsonSerializer.Deserialize<IEnumerable<Notification>>(json);
            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            Assert.NotNull(notifications);
        }
    };
}

