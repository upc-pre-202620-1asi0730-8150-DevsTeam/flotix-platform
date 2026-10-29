using FlotixPlatform.Api.Fleet.Domain.Model.ValueObjects;

namespace FlotixPlatform.API.Fleet.Domain.Model.Commands;

/// <summary>
///     Command to create a vehicle
/// </summary>
/// <param name="OwnerId">The identifier of the owner user.</param>
/// <param name="DriverId">The identifier of the driver user.</param>
/// <param name="LicensePlate">The license plate of the vehicle.</param>
/// <param name="VehicleType">The type of the vehicle (e.g. Camión de carga).</param>
/// <param name="CurrentMileage">The current mileage of the vehicle in kilometers.</param>
/// <param name="Status">The current status of the vehicle.</param>
public abstract record CreateVehicleCommand(Guid OwnerId, Guid? DriverId, 
    LicensePlate LicensePlate, string VehicleType, decimal CurrentMileage, VehicleStatus Status);