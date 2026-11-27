namespace FlotixPlatform.Api.Incidents.Domain.Model.Queries;

/// <summary>
///     Query to get all incidents by driver ID
/// </summary>
/// <param name="DriverId">The driver ID to search</param>
public record GetAllIncidentsByDriverIdQuery(Guid DriverId);
