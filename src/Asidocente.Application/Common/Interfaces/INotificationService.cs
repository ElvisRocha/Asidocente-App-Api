namespace Asidocente.Application.Common.Interfaces;

/// <summary>
/// Push notification service interface (Firebase FCM)
/// </summary>
public interface INotificationService
{
    Task SendNotificationAsync(string userId, string title, string message, CancellationToken cancellationToken = default);
    Task SendBulkNotificationAsync(IEnumerable<string> userIds, string title, string message, CancellationToken cancellationToken = default);
    Task SendTopicNotificationAsync(string topic, string title, string message, CancellationToken cancellationToken = default);
}
