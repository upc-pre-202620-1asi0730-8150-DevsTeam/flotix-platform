using FlotixPlatform.Api.Analytics.Domain.Model.ValueObjects;

namespace FlotixPlatform.Api.Analytics.Interfaces.Rest.Resources;

/// <summary>
///     Represents the data required to generate a fuel or maintenance report.
/// </summary>
/// <param name="OwnerId">The identifier of the owner user.</param>
/// <param name="ReportType">The type of the report.</param>
/// <param name="From">The first day of the period (inclusive).</param>
/// <param name="To">The last day of the period (inclusive).</param>
public record GenerateReportResource(
    Guid OwnerId,
    ReportType ReportType,
    DateOnly From,
    DateOnly To);
