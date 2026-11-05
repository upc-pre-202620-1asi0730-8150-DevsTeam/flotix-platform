using FlotixPlatform.Api.Incidents.Domain.Model.ValueObjects;

namespace FlotixPlatform.Api.Incidents.Domain.Model.Commands;

/// <summary>
///     Command to update an incident
/// </summary>
/// <param name="IncidentId">The identifier of the incident to update</param>
/// <param name="Description">The description of the incident.</param>
/// <param name="Status">The current status of the incident.</param>
public record UpdateIncidentCommand(Guid IncidentId, string Description, IncidentStatus Status);
