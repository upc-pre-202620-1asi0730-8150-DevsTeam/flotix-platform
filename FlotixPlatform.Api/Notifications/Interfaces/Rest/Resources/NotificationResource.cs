namespace FlotixPlatform.Api.Notifications.Interfaces.Rest.Resources;

/// <summary>
///     Represents the data provided by the server about a notification.
/// </summary>
/// <param name="Id">The server-generated ID of the notification</param>
/// <param name="UserId">The identifier of the recipient user.</param>
/// <param name="AlertId">The identifier of the related alert, if any.</param>
/// <param name="Message">The message of the notification.</param>
/// <param name="IsRead">Whether the notification has been read.</param>
/// <param name="SentDate">The moment the notification was sent.</param>
public record NotificationResource(
    Guid Id,
    Guid UserId,
    Guid? AlertId,
    string Message,
    bool IsRead,
    DateTimeOffset SentDate);
