using Microsoft.AspNetCore.SignalR;

namespace FlotixPlatform.Api.Notifications.Interfaces.Hubs;

/// <summary>
///     SignalR hub of the Alerts & Notifications bounded context, mapped at <c>/hubs/notifications</c>.
/// </summary>
/// <remarks>
///     Clients only listen: events are pushed by the REST controllers after a successful command.
/// </remarks>
public class NotificationsHub : Hub<INotificationsClient>;
