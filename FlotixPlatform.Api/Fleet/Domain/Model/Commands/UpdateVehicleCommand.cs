using FlotixPlatform.API.Fleet.Domain.Model.ValueObjects;

namespace FlotixPlatform.API.Fleet.Domain.Model.Commands;

/// <summary>
///     Command to update a vehicle
/// </summary>
/// <param name="VehicleId">The identifier of the vehicle to update</param>
/// <param name="DriverId">The identifier of the driver user.</param>
/// <param name="LicensePlate">The license plate of the vehicle.</param>
/// <param name="VehicleType">The type of the vehicle (e.g. Camión de carga).</param>
/// <param name="CurrentMileage">The current mileage of the vehicle in kilometers.</param>
/// <param name="Status">The current status of the vehicle.</param>
public abstract record UpdateVehicleCommand(Guid VehicleId, Guid? DriverId, 
    LicensePlate LicensePlate, string VehicleType, decimal CurrentMileage, VehicleStatus Status);