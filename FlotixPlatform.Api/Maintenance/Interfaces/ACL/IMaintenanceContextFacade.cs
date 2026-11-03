namespace FlotixPlatform.API.Maintenance.Interfaces.ACL;

/// <summary>
///     Facade (anti-corruption layer) that exposes the Maintenance Management data other bounded contexts need.
/// </summary>
public interface IMaintenanceContextFacade
{
    /// <summary>
    ///     Fetches the maintenance requests of a set of vehicles requested within a period.
    /// </summary>
    /// <param name="vehicleIds">The identifiers of the vehicles.</param>
    /// <param name="from">The start of the period (inclusive).</param>
    /// <param name="to">The end of the period (exclusive).</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>The vehicle, estimated cost, budget approval and completion of each maintenance request.</returns>
    Task<IEnumerable<(Guid VehicleId, decimal EstimatedCost, bool IsBudgetApproved, bool IsCompleted)>> FetchMaintenanceRequestsAsync(
        IReadOnlyCollection<Guid> vehicleIds, DateTimeOffset from, DateTimeOffset to,
        CancellationToken cancellationToken = default);
}
