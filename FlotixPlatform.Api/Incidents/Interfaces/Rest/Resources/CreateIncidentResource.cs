using System.ComponentModel.DataAnnotations;
using FlotixPlatform.Api.Incidents.Domain.Model.ValueObjects;

namespace FlotixPlatform.Api.Incidents.Interfaces.Rest.Resources;

/// <summary>
///     Represents the data required to create an incident.
/// </summary>
/// <param name="VehicleId">The identifier of the vehicle.</param>
/// <param name="DriverId">The identifier of the driver user.</param>
/// <param name="Description">The description of the incident. (required)</param>
/// <param name="Status">The current status of the incident.</param>
public record CreateIncidentResource(
    Guid VehicleId,
    Guid DriverId,
    [Required(ErrorMessage = "RequiredDescription")]
    [StringLength(500, ErrorMessage = "StringLengthField")]
    string Description,
    IncidentStatus Status);
