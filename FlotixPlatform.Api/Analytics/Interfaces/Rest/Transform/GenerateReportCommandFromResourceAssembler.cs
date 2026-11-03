using FlotixPlatform.Api.Analytics.Domain.Model.Commands;
using FlotixPlatform.Api.Analytics.Interfaces.Rest.Resources;

namespace FlotixPlatform.Api.Analytics.Interfaces.Rest.Transform;

/// <summary>
///     Assembles a GenerateReportCommand from a GenerateReportResource.
/// </summary>
public static class GenerateReportCommandFromResourceAssembler
{
    /// <summary>
    ///     Assembles a GenerateReportCommand from a GenerateReportResource.
    /// </summary>
    /// <param name="resource">The GenerateReportResource resource</param>
    /// <returns>
    ///     A GenerateReportCommand assembled from the GenerateReportResource
    /// </returns>
    public static GenerateReportCommand ToCommandFromResource(GenerateReportResource resource) =>
        new(resource.OwnerId, resource.ReportType, resource.From, resource.To);
}
