using FlotixPlatform.Api.Fleet.Domain.Model.Commands;
using FlotixPlatform.Api.Fleet.Domain.Model.ValueObjects;
using FlotixPlatform.Api.Fleet.Interfaces.Rest.Resources;

namespace FlotixPlatform.Api.Fleet.Interfaces.Rest.Transform;

/// <summary>
///     Assembles a CreateVehicleCommand from a CreateVehicleResource.
/// </summary>
public static class CreateVehicleCommandFromResourceAssembler
{
    /// <summary>
    ///     Assembles a CreateVehicleCommand from a CreateVehicleResource.
    /// </summary>
    /// <param name="resource">The CreateVehicleResource resource</param>
    /// <returns>
    ///     A CreateVehicleCommand assembled from the CreateVehicleResource
    /// </returns>
    /// <exception cref="ArgumentException">Thrown when a value object cannot be created from the resource.</exception>
    public static CreateVehicleCommand ToCommandFromResource(CreateVehicleResource resource) =>
        new(resource.OwnerId, resource.DriverId, new LicensePlate(resource.LicensePlate), resource.VehicleType, resource.CurrentMileage, resource.Status);
}