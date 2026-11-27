using FlotixPlatform.Api.Incidents.Domain.Model.ValueObjects;

namespace FlotixPlatform.Api.Incidents.Interfaces.Rest.Resources;

/// <summary>
///     Represents the data provided by the server about an incident.
/// </summary>
/// <param name="Id">The server-generated ID of the incident</param>
/// <param name="VehicleId">The identifier of the vehicle.</param>
/// <param name="DriverId">The identifier of the driver user.</param>
/// <param name="Description">The description of the incident.</param>
/// <param name="Status">The current status of the incident.</param>
/// <param name="ReportedDate">The moment the incident was reported.</param>
public record IncidentResource(
    Guid Id,
    Guid VehicleId,
    Guid DriverId,
    string Description,
    IncidentStatus Status,
    DateTimeOffset ReportedDate);
