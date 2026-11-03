using FlotixPlatform.API.Maintenance.Domain.Model.Aggregates;
using FlotixPlatform.API.Maintenance.Interfaces.Rest.Resources;

namespace FlotixPlatform.API.Maintenance.Interfaces.Rest.Transform;

/// <summary>
///     Assembles a MaintenanceRequestResource from a MaintenanceRequest.
/// </summary>
public static class MaintenanceRequestResourceFromEntityAssembler
{
    /// <summary>
    ///     Assembles a MaintenanceRequestResource from a MaintenanceRequest.
    /// </summary>
    /// <param name="entity">The MaintenanceRequest entity</param>
    /// <returns>
    ///     A MaintenanceRequestResource assembled from the MaintenanceRequest
    /// </returns>
    public static MaintenanceRequestResource ToResourceFromEntity(MaintenanceRequest entity) =>
        new(entity.Id, entity.VehicleId, entity.MechanicId, entity.Description, entity.EstimatedCost, entity.Status, entity.RequestDate);
}
