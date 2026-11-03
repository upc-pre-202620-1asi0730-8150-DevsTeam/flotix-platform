using FlotixPlatform.API.Commerce.Domain.Model.Aggregates;
using FlotixPlatform.API.Shared.Domain.Repositories;

namespace FlotixPlatform.API.Commerce.Domain.Repositories;

/// <summary>
///     The IoT order repository contract.
/// </summary>
public interface IIotOrderRepository : IBaseRepository<IotOrder>
{
    /// <summary>
    ///     Find IoT orders by owner ID
    /// </summary>
    /// <param name="ownerId">The owner ID to search.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>
    ///     An Enumerable containing the IoT order objects if found, or empty otherwise.
    /// </returns>
    Task<IEnumerable<IotOrder>> FindByOwnerIdAsync(Guid ownerId, CancellationToken cancellationToken = default);
}
