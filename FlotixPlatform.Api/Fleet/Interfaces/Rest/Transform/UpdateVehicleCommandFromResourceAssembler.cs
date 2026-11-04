using FlotixPlatform.Api.Fleet.Domain.Model.Commands;
using FlotixPlatform.Api.Fleet.Domain.Model.ValueObjects;
using FlotixPlatform.Api.Fleet.Interfaces.Rest.Resources;
using FlotixPlatform.Api.Fleet.Interfaces.Rest.Resources;

namespace FlotixPlatform.Api.Fleet.Interfaces.Rest.Transform;

/// <summary>
///     Assembles an UpdateVehicleCommand from an UpdateVehicleResource.
/// </summary>
public static class UpdateVehicleCommandFromResourceAssembler
{
    /// <summary>
    ///     Assembles an UpdateVehicleCommand from an UpdateVehicleResource.
    /// </summary>
    /// <param name="vehicleId">The identifier of the vehicle to update</param>
    /// <param name="resource">The UpdateVehicleResource resource</param>
    /// <returns>
    ///     An UpdateVehicleCommand assembled from the UpdateVehicleResource
    /// </returns>
    /// <exception cref="ArgumentException">Thrown when a value object cannot be created from the resource.</exception>
    public static UpdateVehicleCommand ToCommandFromResource(Guid vehicleId, UpdateVehicleResource resource) =>
        new(vehicleId,
            resource.DriverId,
            new LicensePlate(resource.LicensePlate),
            resource.VehicleType,
            resource.CurrentMileage,
            resource.Status);
}