using FlotixPlatform.Api.Notifications.Domain.Model.Aggregates;
using FlotixPlatform.Api.Notifications.Application.Services;
using FlotixPlatform.Api.Notifications.Domain.Model.Queries;
using FlotixPlatform.Api.Notifications.Domain.Repositories;

namespace FlotixPlatform.Api.Notifications.Application.Internal.QueryServices;

/// <summary>
///     Application service for querying notifications.
/// </summary>
/// <remarks>
///     This service acts as the query handler for retrieving notifications.
///     All queries delegate to the repository layer for data access.
/// </remarks>
/// <param name="notificationRepository">Repository for accessing notification data.</param>
public class NotificationQueryService(INotificationRepository notificationRepository)
    : INotificationQueryService
{
    /// <inheritdoc />
    public async Task<IEnumerable<Notification>> Handle(GetAllNotificationsQuery query,
        CancellationToken cancellationToken = default) =>
        await notificationRepository.ListAsync(cancellationToken);

    /// <inheritdoc />
    public async Task<Notification?> Handle(GetNotificationByIdQuery query,
        CancellationToken cancellationToken = default) =>
        await notificationRepository.FindByIdAsync(query.Id, cancellationToken);

    /// <inheritdoc />
    public async Task<IEnumerable<Notification>> Handle(GetAllNotificationsByUserIdQuery query,
        CancellationToken cancellationToken = default) =>
        await notificationRepository.FindByUserIdAsync(query.UserId, cancellationToken);
}
