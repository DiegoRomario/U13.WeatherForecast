#region Services Config
using U13.WeatherForecast.MinimalAPI.Configs;
using U13.WeatherForecast.MinimalAPI.Models;
using U13.WeatherForecast.MinimalAPI.Models.WeatherForecast;
using U13.WeatherForecast.MinimalAPI.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.Configure<HttpClientSettings>(builder.Configuration.GetSection("HttpClient"));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddTransient<HttpClient>();
builder.Services.AddScoped<INotificationService, NotificationService>();
builder.Services.AddScoped<IGeoCodingHttpService, GeoCodingHttpService>();
builder.Services.AddScoped<IWeatherHttpService, WeatherHttpService>();
builder.Services.AddScoped<IWeatherForecastService, WeatherForecastService>();

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

app.MapGet("/getWeatherForecastByAddress", async (string address, IWeatherForecastService weatherForecastService, INotificationService notificationHandlerService) =>
 {
     try
     {
         IEnumerable<Period> weatherForecastForTheNext7Days = await weatherForecastService.GetWeatherForecastForTheNext7DaysByAddress(address);
         if (notificationHandlerService.HasNotification())
             return Results.NotFound(notificationHandlerService.GetNotifications());
         else
             return Results.Ok(weatherForecastForTheNext7Days);
     }
     catch
     {
         return Results.StatusCode(500);
     }
 })
.WithName("GetWeatherForecastByAddress")
.WithTags("Weather")
.Produces<IEnumerable<Period>>(StatusCodes.Status200OK)
.Produces<List<Notification>>(StatusCodes.Status404NotFound);

app.Run();

#endregion