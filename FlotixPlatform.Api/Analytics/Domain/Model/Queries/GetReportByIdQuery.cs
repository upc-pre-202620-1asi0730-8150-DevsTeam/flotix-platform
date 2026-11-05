namespace FlotixPlatform.Api.Analytics.Domain.Model.Queries;

/// <summary>
///     Query to get a report by ID
/// </summary>
/// <param name="Id">The report ID to search</param>
public record GetReportByIdQuery(Guid Id);
