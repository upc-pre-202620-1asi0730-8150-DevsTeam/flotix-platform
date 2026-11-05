namespace FlotixPlatform.Api.Incidents.Domain.Model.Queries;

/// <summary>
///     Query to get an incident by ID
/// </summary>
/// <param name="Id">The incident ID to search</param>
public record GetIncidentByIdQuery(Guid Id);
