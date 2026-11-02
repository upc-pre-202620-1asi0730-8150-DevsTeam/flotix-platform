namespace FlotixPlatform.API.Maintenance.Domain.Model.Commands;

/// <summary>
///     Command to delete a maintenance request
/// </summary>
/// <param name="MaintenanceRequestId">The identifier of the maintenance request to delete</param>
public record DeleteMaintenanceRequestCommand(Guid MaintenanceRequestId);
