namespace FlotixPlatform.API.Maintenance.Domain.Model.Commands;

/// <summary>
///     Command to submit a maintenance request for a vehicle to a mechanic
/// </summary>
/// <param name="VehicleId">The identifier of the vehicle.</param>
/// <param name="MechanicId">The identifier of the mechanic user that attends the request.</param>
/// <param name="Description">The description of the fault to repair.</param>
public record SubmitMaintenanceRequestCommand(Guid VehicleId, Guid MechanicId, string Description);
