using FlotixPlatform.Api.Fleet.Domain.Model.ValueObjects;

namespace FlotixPlatform.Api.Fleet.Interfaces.Rest.Resources;

/// <summary>
///     Represents the data provided by the server about a vehicle.
/// </summary>
/// <param name="Id">The server-generated ID of the vehicle</param>
/// <param name="OwnerId">The identifier of the owner user.</param>
/// <param name="DriverId">The identifier of the driver user.</param>
/// <param name="LicensePlate">The license plate of the vehicle.</param>
/// <param name="VehicleType">The type of the vehicle (e.g. Camión de carga).</param>
/// <param name="CurrentMileage">The current mileage of the vehicle in kilometers.</param>
/// <param name="Status">The current status of the vehicle.</param>
public record VehicleResource(
    Guid Id,
    Guid OwnerId,
    Guid? DriverId,
    string LicensePlate,
    string VehicleType,
    decimal CurrentMileage,
    VehicleStatus Status);