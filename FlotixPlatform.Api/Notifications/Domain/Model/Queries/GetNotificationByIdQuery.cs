namespace FlotixPlatform.Api.Notifications.Domain.Model.Queries;

/// <summary>
///     Query to get a notification by ID
/// </summary>
/// <param name="Id">The notification ID to search</param>
public record GetNotificationByIdQuery(Guid Id);
