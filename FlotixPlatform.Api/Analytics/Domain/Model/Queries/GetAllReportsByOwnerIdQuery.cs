namespace FlotixPlatform.Api.Analytics.Domain.Model.Queries;

/// <summary>
///     Query to get all reports by owner ID
/// </summary>
/// <param name="OwnerId">The owner ID to search</param>
public record GetAllReportsByOwnerIdQuery(Guid OwnerId);
