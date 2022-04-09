#region Services Config
using U13.WeatherForecast.MinimalAPI.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<HttpClient>();
builder.Services.AddScoped<IGeoCodingService, GeoCodingService>();
builder.Services.AddScoped<IWeatherService, WeatherService>();

var app = builder.Build();
#endregion

#region Pipeline Config
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
#endregion

#region End Points

app.MapGet("/getWeatherForecastByAddress", async (string address, IGeoCodingService geoCodingService, IWeatherService weatherService) =>
 {
     var geoCoding = await geoCodingService.GetGeoCodingByAddress(address);
     var coodinates = geoCoding.Result.AddressMatches?.FirstOrDefault().Coordinates;
     var gridPoints = await weatherService.GetGridPointByCoordinates(coodinates.X, coodinates.Y);
     var weatherForecast = await weatherService.GetWeatherForecastByGrid(gridPoints.Properties.GridId, gridPoints.Properties.GridX, gridPoints.Properties.GridY);
     var WeatherForecastForNowAndNext7Days = weatherForecast.Properties.Periods.Take(8);
     return Results.Ok(WeatherForecastForNowAndNext7Days);
 })
.WithName("GetWeatherForecastByAddress");

app.Run();

#endregion