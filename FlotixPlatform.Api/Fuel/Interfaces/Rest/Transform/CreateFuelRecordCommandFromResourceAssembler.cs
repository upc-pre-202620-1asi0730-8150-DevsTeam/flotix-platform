using FlotixPlatform.Api.Fuel.Domain.Model.Commands;
using FlotixPlatform.Api.Fuel.Interfaces.Rest.Resources;

namespace FlotixPlatform.Api.Fuel.Interfaces.Rest.Transform;

/// <summary>
///     Assembles a CreateFuelRecordCommand from a CreateFuelRecordResource.
/// </summary>
public static class CreateFuelRecordCommandFromResourceAssembler
{
    /// <summary>
    ///     Assembles a CreateFuelRecordCommand from a CreateFuelRecordResource.
    /// </summary>
    /// <param name="resource">The CreateFuelRecordResource resource</param>
    /// <returns>
    ///     A CreateFuelRecordCommand assembled from the CreateFuelRecordResource
    /// </returns>
    /// <exception cref="ArgumentException">Thrown when a value object cannot be created from the resource.</exception>
    public static CreateFuelRecordCommand ToCommandFromResource(CreateFuelRecordResource resource) =>
        new(resource.VehicleId, resource.DriverId, resource.Liters, resource.Cost, resource.MileageAtRefill, resource.RecordedDate);
}
