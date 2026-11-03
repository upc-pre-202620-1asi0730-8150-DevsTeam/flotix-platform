namespace FlotixPlatform.API.Commerce.Domain.Model.Queries;

/// <summary>
///     Query to get all IoT orders by owner ID
/// </summary>
/// <param name="OwnerId">The owner ID to search</param>
public record GetAllIotOrdersByOwnerIdQuery(Guid OwnerId);
