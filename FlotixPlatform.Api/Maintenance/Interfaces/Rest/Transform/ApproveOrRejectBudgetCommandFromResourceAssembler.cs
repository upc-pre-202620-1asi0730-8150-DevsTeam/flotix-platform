using FlotixPlatform.API.Maintenance.Domain.Model.Commands;
using FlotixPlatform.API.Maintenance.Interfaces.Rest.Resources;

namespace FlotixPlatform.API.Maintenance.Interfaces.Rest.Transform;

/// <summary>
///     Assembles an ApproveOrRejectBudgetCommand from an ApproveOrRejectBudgetResource.
/// </summary>
public static class ApproveOrRejectBudgetCommandFromResourceAssembler
{
    /// <summary>
    ///     Assembles an ApproveOrRejectBudgetCommand from an ApproveOrRejectBudgetResource.
    /// </summary>
    /// <param name="maintenanceRequestId">The identifier of the maintenance request</param>
    /// <param name="resource">The ApproveOrRejectBudgetResource resource</param>
    /// <returns>
    ///     An ApproveOrRejectBudgetCommand assembled from the ApproveOrRejectBudgetResource
    /// </returns>
    public static ApproveOrRejectBudgetCommand ToCommandFromResource(Guid maintenanceRequestId,
        ApproveOrRejectBudgetResource resource) =>
        new(maintenanceRequestId, resource.IsApproved);
}
