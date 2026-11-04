using FlotixPlatform.Api.Maintenance.Domain.Repositories;
using FlotixPlatform.Api.Maintenance.Interfaces.Acl;

namespace FlotixPlatform.Api.Maintenance.Application.Acl;

/// <summary>
///     Implementation of the Maintenance Management context facade.
/// </summary>
/// <param name="maintenanceRequestRepository">Repository for accessing maintenance request data.</param>
public class MaintenanceContextFacade(IMaintenanceRequestRepository maintenanceRequestRepository) : IMaintenanceContextFacade
{
    /// <inheritdoc />
    public async Task<IEnumerable<(Guid VehicleId, decimal EstimatedCost, bool IsBudgetApproved, bool IsCompleted)>> FetchMaintenanceRequestsAsync(
        IReadOnlyCollection<Guid> vehicleIds, DateTimeOffset from, DateTimeOffset to,
        CancellationToken cancellationToken = default)
    {
        var maintenanceRequests = await maintenanceRequestRepository.FindByVehicleIdsAndRequestDateRangeAsync(vehicleIds,
            from, to, cancellationToken);
        return maintenanceRequests.Select(request =>
            (request.VehicleId, request.EstimatedCost, request.IsBudgetApproved, request.IsCompleted));
    }
}
