namespace FlotixPlatform.API.Maintenance.Domain.Model.Queries;

/// <summary>
///     Query to get a maintenance request by ID
/// </summary>
/// <param name="Id">The maintenance request ID to search</param>
public record GetMaintenanceRequestByIdQuery(Guid Id);
