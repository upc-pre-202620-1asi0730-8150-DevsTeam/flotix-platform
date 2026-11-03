namespace FlotixPlatform.Api.Notifications.Domain.Model.Queries;

/// <summary>
///     Query to get all notifications by user ID
/// </summary>
/// <param name="UserId">The user ID to search</param>
public record GetAllNotificationsByUserIdQuery(Guid UserId);
