namespace FlotixPlatform.Api.Incidents.Domain.Model.Queries;

/// <summary>
///     Query to get all incidents by vehicle ID
/// </summary>
/// <param name="VehicleId">The vehicle ID to search</param>
public record GetAllIncidentsByVehicleIdQuery(Guid VehicleId);
