using FlotixPlatform.Api.Analytics.Domain.Model.Aggregates;
using FlotixPlatform.Api.Analytics.Domain.Model.ValueObjects;
using FlotixPlatform.Api.Analytics.Interfaces.Rest.Resources;

namespace FlotixPlatform.Api.Analytics.Interfaces.Rest.Transform;

/// <summary>
///     Assembles a GeneratedReportResource from a Report and its lines.
/// </summary>
public static class GeneratedReportResourceFromResultAssembler
{
    /// <summary>
    ///     Assembles a GeneratedReportResource from a Report and its lines.
    /// </summary>
    /// <param name="report">The stored Report entity</param>
    /// <param name="lines">The figures of each vehicle</param>
    /// <param name="noRecordsMessage">The informative message returned when there are no records</param>
    /// <returns>
    ///     A GeneratedReportResource assembled from the Report and its lines
    /// </returns>
    public static GeneratedReportResource ToResourceFromResult(Report report, IReadOnlyList<ReportLine> lines,
        string noRecordsMessage) =>
        new(report.Id, report.OwnerId, report.ReportType, report.GeneratedDate, report.FilterParams,
            lines.Select(line => new ReportLineResource(line.VehicleId, line.LicensePlate, line.Records, line.Quantity,
                line.TotalCost)),
            lines.Sum(line => line.Records),
            lines.Sum(line => line.Quantity),
            lines.Sum(line => line.TotalCost),
            lines.Count == 0 ? noRecordsMessage : null);
}
