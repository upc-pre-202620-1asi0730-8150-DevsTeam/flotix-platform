namespace FlotixPlatform.API.Maintenance.Domain.Model.Commands;

/// <summary>
///     Command to register the completion of the repair of a maintenance request
/// </summary>
/// <param name="MaintenanceRequestId">The identifier of the maintenance request.</param>
public record CompleteMaintenanceCommand(Guid MaintenanceRequestId);
