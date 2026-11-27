namespace FlotixPlatform.Api.Incidents.Domain.Model.Commands;

/// <summary>
///     Command to delete an incident
/// </summary>
/// <param name="IncidentId">The identifier of the incident to delete</param>
public record DeleteIncidentCommand(Guid IncidentId);
