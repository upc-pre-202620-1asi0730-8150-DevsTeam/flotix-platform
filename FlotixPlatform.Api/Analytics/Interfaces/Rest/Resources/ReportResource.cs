using FlotixPlatform.Api.Analytics.Domain.Model.ValueObjects;

namespace FlotixPlatform.Api.Analytics.Interfaces.Rest.Resources;

/// <summary>
///     Represents the data provided by the server about a report.
/// </summary>
/// <param name="Id">The server-generated ID of the report</param>
/// <param name="OwnerId">The identifier of the owner user.</param>
/// <param name="ReportType">The type of the report.</param>
/// <param name="GeneratedDate">The moment the report was generated.</param>
/// <param name="FilterParams">The filters applied to the report (e.g. date range).</param>
public record ReportResource(
    Guid Id,
    Guid OwnerId,
    ReportType ReportType,
    DateTimeOffset GeneratedDate,
    string FilterParams);
