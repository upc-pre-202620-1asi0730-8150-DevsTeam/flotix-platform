namespace FlotixPlatform.Api.Notifications.Interfaces.Rest.Resources;

/// <summary>
///     Represents the data required to update a notification.
/// </summary>
/// <param name="IsRead">Whether the notification has been read.</param>
public record UpdateNotificationResource(
    bool IsRead);
