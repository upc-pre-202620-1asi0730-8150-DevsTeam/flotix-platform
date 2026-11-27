using FlotixPlatform.Api.Notifications.Domain.Model.Aggregates;
using FlotixPlatform.Api.Notifications.Domain.Model.Queries;

namespace FlotixPlatform.Api.Notifications.Application.Services;

/// <summary>
///     Query service interface for notification operations.
/// </summary>
/// <remarks>
///     This interface defines the contract for querying notifications by various filters.
/// </remarks>
public interface INotificationQueryService
{
	/// <summary>
	///     Handle the GetAllNotificationsQuery.
	/// </summary>
	/// <remarks>
	///     This method handles the GetAllNotificationsQuery. It returns all the notifications.
	/// </remarks>
	/// <param name="query">The GetAllNotificationsQuery query</param>
	/// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
	/// <returns>An IEnumerable containing the Notification objects</returns>
	Task<IEnumerable<Notification>> Handle(GetAllNotificationsQuery query, CancellationToken cancellationToken = default);

	/// <summary>
	///     Handle the GetNotificationByIdQuery.
	/// </summary>
	/// <remarks>
	///     This method handles the GetNotificationByIdQuery. It returns the notification for the given Id.
	/// </remarks>
	/// <param name="query">The GetNotificationByIdQuery query</param>
	/// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
	/// <returns>The Notification object if found, or null otherwise</returns>
	Task<Notification?> Handle(GetNotificationByIdQuery query, CancellationToken cancellationToken = default);

	/// <summary>
	///     Handle the GetAllNotificationsByUserIdQuery.
	/// </summary>
	/// <remarks>
	///     This method handles the GetAllNotificationsByUserIdQuery. It returns all the notifications for the given user ID.
	/// </remarks>
	/// <param name="query">The GetAllNotificationsByUserIdQuery query</param>
	/// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
	/// <returns>An IEnumerable containing the Notification objects</returns>
	Task<IEnumerable<Notification>> Handle(GetAllNotificationsByUserIdQuery query, CancellationToken cancellationToken = default);
}
