using FlotixPlatform.API.Maintenance.Domain.Repositories;
using FlotixPlatform.API.Maintenance.Interfaces.ACL;

namespace FlotixPlatform.API.Maintenance.Application.ACL;

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
