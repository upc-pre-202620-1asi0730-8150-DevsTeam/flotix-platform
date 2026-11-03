namespace FlotixPlatform.Api.Notifications.Domain.Model.Commands;

/// <summary>
///     Command to update a notification
/// </summary>
/// <param name="NotificationId">The identifier of the notification to update</param>
/// <param name="IsRead">Whether the notification has been read.</param>
public record UpdateNotificationCommand(Guid NotificationId, bool IsRead);
