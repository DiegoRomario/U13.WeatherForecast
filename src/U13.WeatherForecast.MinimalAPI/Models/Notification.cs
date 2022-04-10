namespace U13.WeatherForecast.MinimalAPI.Models
{
    public class Notification
    {
        public DateTime Timestamp { get; private set; }
        public Notification(string message)
        {
            Timestamp = DateTime.Now;
            Message = message;
        }
        public string Message { get; }
    }
}
