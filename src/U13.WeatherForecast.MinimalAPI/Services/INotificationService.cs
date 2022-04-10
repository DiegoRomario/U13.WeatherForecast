using U13.WeatherForecast.MinimalAPI.Models;

namespace U13.WeatherForecast.MinimalAPI.Services
{
    public interface INotificationService
    {
        List<Notification> GetNotifications();
        Task AddNotification(Notification notification);
        bool HasNotification();
    }
}