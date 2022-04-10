using U13.WeatherForecast.MinimalAPI.Models;

namespace U13.WeatherForecast.MinimalAPI.Services
{
    public class NotificationService : INotificationService
    {
        private readonly List<Notification> notifications;
        public NotificationService()
        {
            notifications = new List<Notification>();
        }
        public Task AddNotification(Notification notification)
        {
            notifications.Add(notification);
            return Task.CompletedTask;
        }
        public List<Notification> GetNotifications()
        {
            return notifications;
        }
        public bool HasNotification()
        {
            return GetNotifications().Any();
        }
    }
}

