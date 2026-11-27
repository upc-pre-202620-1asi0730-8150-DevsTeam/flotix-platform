using FlotixPlatform.Api.Notifications.Interfaces.Rest.Resources;

namespace FlotixPlatform.Api.Notifications.Interfaces.Hubs;

/// <summary>
///     Defines the server-to-client events of the <see cref="NotificationsHub"/>.
/// </summary>
public interface INotificationsClient
{
    /// <summary>
    ///     Receives an alert as soon as it is created; sent to every connected client.
    /// </summary>
    /// <param name="alert">The created alert.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task AlertActivated(AlertResource alert);

    /// <summary>
    ///     Receives a notification as soon as it is created; sent to its recipient user.
    /// </summary>
    /// <param name="notification">The created notification.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task NotificationReceived(NotificationResource notification);
}
