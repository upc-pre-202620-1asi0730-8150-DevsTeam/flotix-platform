namespace FlotixPlatform.API.Maintenance.Domain.Model.Commands;

/// <summary>
///     Command to approve or reject the budget quoted for a maintenance request
/// </summary>
/// <param name="MaintenanceRequestId">The identifier of the maintenance request.</param>
/// <param name="IsApproved">True to approve the budget, false to reject it.</param>
public record ApproveOrRejectBudgetCommand(Guid MaintenanceRequestId, bool IsApproved);
