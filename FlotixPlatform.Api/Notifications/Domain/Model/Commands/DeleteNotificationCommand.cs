namespace FlotixPlatform.Api.Notifications.Domain.Model.Commands;

/// <summary>
///     Command to delete a notification
/// </summary>
/// <param name="NotificationId">The identifier of the notification to delete</param>
public record DeleteNotificationCommand(Guid NotificationId);
