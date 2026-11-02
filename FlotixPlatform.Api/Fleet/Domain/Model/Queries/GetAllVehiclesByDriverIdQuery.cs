namespace FlotixPlatform.API.Fleet.Domain.Model.Queries;

/// <summary>
///     Query to get all vehicles by driver ID
/// </summary>
/// <param name="DriverId">The driver ID to search</param>
public record GetAllVehiclesByDriverIdQuery(Guid DriverId);