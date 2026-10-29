namespace FlotixPlatform.Api.Fleet.Domain.Queries;

/// <summary>
///     Query to get all vehicles by owner ID
/// </summary>
/// <param name="OwnerId">The owner ID to search</param>
public record GetAllVehiclesByOwnerIdQuery(Guid OwnerId);