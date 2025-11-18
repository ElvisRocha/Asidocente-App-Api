using Asidocente.Application.Common.Interfaces;
using Microsoft.Extensions.Logging;

namespace Asidocente.Infrastructure.Services;

/// <summary>
/// Push notification service implementation (placeholder for Firebase FCM)
/// </summary>
public class NotificationService : INotificationService
{
    private readonly ILogger<NotificationService> _logger;

    public NotificationService(ILogger<NotificationService> logger)
    {
        _logger = logger;
    }

    public async Task SendNotificationAsync(string userId, string title, string message, CancellationToken cancellationToken = default)
    {
        // TODO: Implement Firebase FCM integration
        _logger.LogInformation("Sending notification to user {UserId}: {Title}", userId, title);
        await Task.CompletedTask;
    }

    public async Task SendBulkNotificationAsync(IEnumerable<string> userIds, string title, string message, CancellationToken cancellationToken = default)
    {
        // TODO: Implement Firebase FCM bulk notification
        _logger.LogInformation("Sending bulk notification to {Count} users: {Title}", userIds.Count(), title);
        await Task.CompletedTask;
    }

    public async Task SendTopicNotificationAsync(string topic, string title, string message, CancellationToken cancellationToken = default)
    {
        // TODO: Implement Firebase FCM topic notification
        _logger.LogInformation("Sending topic notification to {Topic}: {Title}", topic, title);
        await Task.CompletedTask;
    }
}
