namespace FlotixPlatform.API.Maintenance.Interfaces.Rest.Resources;

/// <summary>
///     Represents the decision of the owner about the quoted budget.
/// </summary>
/// <param name="IsApproved">True to approve the budget, false to reject it.</param>
public record ApproveOrRejectBudgetResource(bool IsApproved);
