using FlotixPlatform.Api.Notifications.Domain.Model.Aggregates;
using FlotixPlatform.Api.Shared.Domain.Repositories;

namespace FlotixPlatform.Api.Notifications.Domain.Repositories;

/// <summary>
///     The notification repository contract.
/// </summary>
public interface INotificationRepository : IBaseRepository<Notification>
{
    /// <summary>
    ///     Find notifications by user ID
    /// </summary>
    /// <param name="userId">The user ID to search.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>
    ///     An Enumerable containing the notification objects if found, or empty otherwise.
    /// </returns>
    Task<IEnumerable<Notification>> FindByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
}
