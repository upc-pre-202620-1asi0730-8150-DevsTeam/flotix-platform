namespace FlotixPlatform.Api.Analytics.Domain.Model.Commands;

/// <summary>
///     Command to delete a report
/// </summary>
/// <param name="ReportId">The identifier of the report to delete</param>
public record DeleteReportCommand(Guid ReportId);
