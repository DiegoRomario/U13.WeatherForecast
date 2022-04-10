namespace U13.WeatherForecast.MinimalAPI.Configs
{
    public class HttpClientSettings
    {
        public string GeocodingBase { get; set; }
        public string GeocodingByAddress { get; set; }
        public string WeatherBase { get; set; }
        public string GridPointsByCoordinates { get; set; }
        public string ForecastByGrid { get; set; }

    }
}