namespace FlotixPlatform.API.Maintenance.Domain.Model.Queries;

/// <summary>
///     Query to get all maintenance requests by vehicle ID
/// </summary>
/// <param name="VehicleId">The vehicle ID to search</param>
public record GetAllMaintenanceRequestsByVehicleIdQuery(Guid VehicleId);
