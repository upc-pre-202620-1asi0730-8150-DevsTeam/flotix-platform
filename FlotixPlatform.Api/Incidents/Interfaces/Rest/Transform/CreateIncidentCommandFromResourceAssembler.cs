using FlotixPlatform.Api.Incidents.Domain.Model.Commands;
using FlotixPlatform.Api.Incidents.Interfaces.Rest.Resources;

namespace FlotixPlatform.Api.Incidents.Interfaces.Rest.Transform;

/// <summary>
///     Assembles a CreateIncidentCommand from a CreateIncidentResource.
/// </summary>
public static class CreateIncidentCommandFromResourceAssembler
{
    /// <summary>
    ///     Assembles a CreateIncidentCommand from a CreateIncidentResource.
    /// </summary>
    /// <param name="resource">The CreateIncidentResource resource</param>
    /// <returns>
    ///     A CreateIncidentCommand assembled from the CreateIncidentResource
    /// </returns>
    /// <exception cref="ArgumentException">Thrown when a value object cannot be created from the resource.</exception>
    public static CreateIncidentCommand ToCommandFromResource(CreateIncidentResource resource) =>
        new(resource.VehicleId, resource.DriverId, resource.Description, resource.Status);
}
