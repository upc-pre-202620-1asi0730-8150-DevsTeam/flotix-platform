namespace FlotixPlatform.Api.Fuel.Interfaces.Acl;

/// <summary>
///     Facade (anti-corruption layer) that exposes the Fuel Control data other bounded contexts need.
/// </summary>
public interface IFuelContextFacade
{
    /// <summary>
    ///     Fetches the refuels of a set of vehicles recorded within a period.
    /// </summary>
    /// <param name="vehicleIds">The identifiers of the vehicles.</param>
    /// <param name="from">The start of the period (inclusive).</param>
    /// <param name="to">The end of the period (exclusive).</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>The vehicle, liters and cost of each refuel.</returns>
    Task<IEnumerable<(Guid VehicleId, decimal Liters, decimal Cost)>> FetchFuelRecordsAsync(
        IReadOnlyCollection<Guid> vehicleIds, DateTimeOffset from, DateTimeOffset to,
        CancellationToken cancellationToken = default);
}
