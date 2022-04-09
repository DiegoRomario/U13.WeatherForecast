namespace U13.WeatherForecast.MinimalAPI.Models.GridPoints
{
    public class GridPointsResult
    {
        public List<object> Context { get; set; }
        public string Id { get; set; }
        public string Type { get; set; }
        public Geometry Geometry { get; set; }
        public Properties Properties { get; set; }
    }

    public class Geometry
    {
        public string Type { get; set; }
        public List<double> Coordinates { get; set; }
    }

    public class Distance
    {
        public string UnitCode { get; set; }
        public double Value { get; set; }
    }

    public class Bearing
    {
        public string UnitCode { get; set; }
        public int Value { get; set; }
    }

    public class Properties
    {
        public string City { get; set; }
        public string State { get; set; }
        public Distance Distance { get; set; }
        public Bearing Bearing { get; set; }
        public string Id { get; set; }
        public string Type { get; set; }
        public string Cwa { get; set; }
        public string ForecastOffice { get; set; }
        public string GridId { get; set; }
        public int GridX { get; set; }
        public int GridY { get; set; }
        public string Forecast { get; set; }
        public string ForecastHourly { get; set; }
        public string ForecastGridData { get; set; }
        public string ObservationStations { get; set; }
        public RelativeLocation RelativeLocation { get; set; }
        public string ForecastZone { get; set; }
        public string County { get; set; }
        public string FireWeatherZone { get; set; }
        public string TimeZone { get; set; }
        public string RadarStation { get; set; }
    }

    public class RelativeLocation
    {
        public string Type { get; set; }
        public Geometry Geometry { get; set; }
        public Properties Properties { get; set; }
    }
}
