using FlotixPlatform.Api.Analytics.Domain.Model.Aggregates;
using FlotixPlatform.Api.Shared.Domain.Repositories;

namespace FlotixPlatform.Api.Analytics.Domain.Repositories;

/// <summary>
///     The report repository contract.
/// </summary>
public interface IReportRepository : IBaseRepository<Report>
{
    /// <summary>
    ///     Find reports by owner ID
    /// </summary>
    /// <param name="ownerId">The owner ID to search.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>
    ///     An Enumerable containing the report objects if found, or empty otherwise.
    /// </returns>
    Task<IEnumerable<Report>> FindByOwnerIdAsync(Guid ownerId, CancellationToken cancellationToken = default);
}
