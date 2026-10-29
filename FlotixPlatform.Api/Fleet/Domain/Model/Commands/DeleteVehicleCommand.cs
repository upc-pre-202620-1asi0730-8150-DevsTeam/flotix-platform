namespace FlotixPlatform.API.Fleet.Domain.Model.Commands;

/// <summary>
///     Command to delete a vehicle
/// </summary>
/// <param name="VehicleId">The identifier of the vehicle to delete</param>
public record DeleteVehicleCommand(Guid VehicleId);