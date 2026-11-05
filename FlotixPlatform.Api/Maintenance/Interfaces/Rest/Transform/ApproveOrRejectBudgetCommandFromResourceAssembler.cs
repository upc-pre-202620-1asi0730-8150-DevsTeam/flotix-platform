using FlotixPlatform.Api.Maintenance.Domain.Model.Commands;
using FlotixPlatform.Api.Maintenance.Interfaces.Rest.Resources;

namespace FlotixPlatform.Api.Maintenance.Interfaces.Rest.Transform;

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
