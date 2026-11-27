using FlotixPlatform.Api.Incidents.Domain.Model.Commands;
using FlotixPlatform.Api.Incidents.Interfaces.Rest.Resources;

namespace FlotixPlatform.Api.Incidents.Interfaces.Rest.Transform;

/// <summary>
///     Assembles an UpdateIncidentCommand from an UpdateIncidentResource.
/// </summary>
public static class UpdateIncidentCommandFromResourceAssembler
{
    /// <summary>
    ///     Assembles an UpdateIncidentCommand from an UpdateIncidentResource.
    /// </summary>
    /// <param name="incidentId">The identifier of the incident to update</param>
    /// <param name="resource">The UpdateIncidentResource resource</param>
    /// <returns>
    ///     An UpdateIncidentCommand assembled from the UpdateIncidentResource
    /// </returns>
    /// <exception cref="ArgumentException">Thrown when a value object cannot be created from the resource.</exception>
    public static UpdateIncidentCommand ToCommandFromResource(Guid incidentId, UpdateIncidentResource resource) =>
        new(incidentId, resource.Description, resource.Status);
}
