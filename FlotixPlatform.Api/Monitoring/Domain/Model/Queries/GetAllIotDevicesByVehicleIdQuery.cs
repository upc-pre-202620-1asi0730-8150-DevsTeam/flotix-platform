namespace FlotixPlatform.Api.Monitoring.Domain.Model.Queries;

/// <summary>
///     Query to get all IoT devices by vehicle ID
/// </summary>
/// <param name="VehicleId">The vehicle ID to search</param>
public record GetAllIotDevicesByVehicleIdQuery(Guid VehicleId);
