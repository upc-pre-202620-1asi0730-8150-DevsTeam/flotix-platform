namespace FlotixPlatform.Api.Notifications.Domain.Model.Queries;

/// <summary>
///     Query to get an alert by ID
/// </summary>
/// <param name="Id">The alert ID to search</param>
public record GetAlertByIdQuery(Guid Id);
