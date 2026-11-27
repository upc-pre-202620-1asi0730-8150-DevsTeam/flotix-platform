using FlotixPlatform.Api.Analytics.Domain.Model.Aggregates;
using FlotixPlatform.Api.Analytics.Interfaces.Rest.Resources;

namespace FlotixPlatform.Api.Analytics.Interfaces.Rest.Transform;

/// <summary>
///     Assembles a ReportResource from a Report.
/// </summary>
public static class ReportResourceFromEntityAssembler
{
    /// <summary>
    ///     Assembles a ReportResource from a Report.
    /// </summary>
    /// <param name="entity">The Report entity</param>
    /// <returns>
    ///     A ReportResource assembled from the Report
    /// </returns>
    public static ReportResource ToResourceFromEntity(Report entity) =>
        new(entity.Id, entity.OwnerId, entity.ReportType, entity.GeneratedDate, entity.FilterParams);
}
