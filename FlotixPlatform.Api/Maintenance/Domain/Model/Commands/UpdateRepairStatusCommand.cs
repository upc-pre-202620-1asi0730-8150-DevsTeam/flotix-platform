namespace FlotixPlatform.API.Maintenance.Domain.Model.Commands;

/// <summary>
///     Command to register that the workshop started the approved repair
/// </summary>
/// <param name="MaintenanceRequestId">The identifier of the maintenance request.</param>
public record UpdateRepairStatusCommand(Guid MaintenanceRequestId);
