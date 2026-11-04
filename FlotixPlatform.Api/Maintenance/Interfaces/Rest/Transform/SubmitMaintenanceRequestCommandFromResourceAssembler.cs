using FlotixPlatform.API.Maintenance.Domain.Model.Commands;
using FlotixPlatform.API.Maintenance.Interfaces.Rest.Resources;

namespace FlotixPlatform.API.Maintenance.Interfaces.Rest.Transform;

/// <summary>
///     Assembles a SubmitMaintenanceRequestCommand from a SubmitMaintenanceRequestResource.
/// </summary>
public static class SubmitMaintenanceRequestCommandFromResourceAssembler
{
    /// <summary>
    ///     Assembles a SubmitMaintenanceRequestCommand from a SubmitMaintenanceRequestResource.
    /// </summary>
    /// <param name="resource">The SubmitMaintenanceRequestResource resource</param>
    /// <returns>
    ///     A SubmitMaintenanceRequestCommand assembled from the SubmitMaintenanceRequestResource
    /// </returns>
    public static SubmitMaintenanceRequestCommand ToCommandFromResource(SubmitMaintenanceRequestResource resource) =>
        new(resource.VehicleId, resource.MechanicId, resource.Description);
}
