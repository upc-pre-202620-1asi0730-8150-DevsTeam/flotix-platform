using FlotixPlatform.API.Maintenance.Domain.Model.ValueObjects;

namespace FlotixPlatform.API.Maintenance.Interfaces.Rest.Resources;

/// <summary>
///     Represents the data provided by the server about a maintenance request.
/// </summary>
/// <param name="Id">The server-generated ID of the maintenance request</param>
/// <param name="VehicleId">The identifier of the vehicle.</param>
/// <param name="MechanicId">The identifier of the mechanic user.</param>
/// <param name="Description">The description of the fault to repair.</param>
/// <param name="EstimatedCost">The estimated cost quoted by the workshop.</param>
/// <param name="Status">The current status of the maintenance request.</param>
/// <param name="RequestDate">The moment the maintenance was requested.</param>
public record MaintenanceRequestResource(
    Guid Id,
    Guid VehicleId,
    Guid MechanicId,
    string Description,
    decimal EstimatedCost,
    MaintenanceRequestStatus Status,
    DateTimeOffset RequestDate);
