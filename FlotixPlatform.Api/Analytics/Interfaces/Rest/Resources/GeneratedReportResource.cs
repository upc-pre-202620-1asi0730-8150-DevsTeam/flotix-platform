using FlotixPlatform.Api.Analytics.Domain.Model.ValueObjects;

namespace FlotixPlatform.Api.Analytics.Interfaces.Rest.Resources;

/// <summary>
///     Represents a generated report with the figures of each vehicle and the totals of the fleet.
/// </summary>
/// <param name="Id">The server-generated ID of the report</param>
/// <param name="OwnerId">The identifier of the owner user</param>
/// <param name="ReportType">The type of the report</param>
/// <param name="GeneratedDate">The moment the report was generated</param>
/// <param name="FilterParams">The filters applied to the report (date range)</param>
/// <param name="Lines">The figures of each vehicle with records in the period</param>
/// <param name="TotalRecords">The total number of records of the fleet in the period</param>
/// <param name="TotalQuantity">The total liters (fuel report) or completed repairs (maintenance report)</param>
/// <param name="TotalCost">The total fuel cost (fuel report) or approved budgets (maintenance report)</param>
/// <param name="Message">An informative message when there are no records in the period; otherwise null</param>
public record GeneratedReportResource(
    Guid Id,
    Guid OwnerId,
    ReportType ReportType,
    DateTimeOffset GeneratedDate,
    string FilterParams,
    IEnumerable<ReportLineResource> Lines,
    int TotalRecords,
    decimal TotalQuantity,
    decimal TotalCost,
    string? Message);
