namespace FlotixPlatform.API.Maintenance.Domain.Model.Queries;

/// <summary>
///     Query to get all maintenance requests by mechanic ID
/// </summary>
/// <param name="MechanicId">The mechanic ID to search</param>
public record GetAllMaintenanceRequestsByMechanicIdQuery(Guid MechanicId);
