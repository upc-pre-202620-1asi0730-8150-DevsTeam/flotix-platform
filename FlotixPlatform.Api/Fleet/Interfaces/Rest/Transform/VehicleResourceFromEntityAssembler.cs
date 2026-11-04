using FlotixPlatform.Api.Fleet.Domain.Model.Aggregates;
using FlotixPlatform.Api.Fleet.Interfaces.Rest.Resources;

namespace FlotixPlatform.Api.Fleet.Interfaces.Rest.Transform;

/// <summary>
///     Assembles a VehicleResource from a Vehicle.
/// </summary>
public static class VehicleResourceFromEntityAssembler
{
    /// <summary>
    ///     Assembles a VehicleResource from a Vehicle.
    /// </summary>
    /// <param name="entity">The Vehicle entity</param>
    /// <returns>
    ///     A VehicleResource assembled from the Vehicle
    /// </returns>
    public static VehicleResource ToResourceFromEntity(Vehicle entity) =>
        new(entity.Id, entity.OwnerId, entity.DriverId, entity.LicensePlate.Value, entity.VehicleType, entity.CurrentMileage, entity.Status);
}