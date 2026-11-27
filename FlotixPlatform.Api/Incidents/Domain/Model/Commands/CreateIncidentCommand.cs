using FlotixPlatform.Api.Incidents.Domain.Model.ValueObjects;

namespace FlotixPlatform.Api.Incidents.Domain.Model.Commands;

/// <summary>
///     Command to create an incident
/// </summary>
/// <param name="VehicleId">The identifier of the vehicle.</param>
/// <param name="DriverId">The identifier of the driver user.</param>
/// <param name="Description">The description of the incident.</param>
/// <param name="Status">The current status of the incident.</param>
public record CreateIncidentCommand(Guid VehicleId, Guid DriverId, string Description, IncidentStatus Status);
