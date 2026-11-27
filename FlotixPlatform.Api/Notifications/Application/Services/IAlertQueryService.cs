using FlotixPlatform.Api.Notifications.Domain.Model.Aggregates;
using FlotixPlatform.Api.Notifications.Domain.Model.Queries;

namespace FlotixPlatform.Api.Notifications.Application.Services;

/// <summary>
///     Query service interface for alert operations.
/// </summary>
/// <remarks>
///     This interface defines the contract for querying alerts by various filters.
/// </remarks>
public interface IAlertQueryService
{
	/// <summary>
	///     Handle the GetAllAlertsQuery.
	/// </summary>
	/// <remarks>
	///     This method handles the GetAllAlertsQuery. It returns all the alerts.
	/// </remarks>
	/// <param name="query">The GetAllAlertsQuery query</param>
	/// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
	/// <returns>An IEnumerable containing the Alert objects</returns>
	Task<IEnumerable<Alert>> Handle(GetAllAlertsQuery query, CancellationToken cancellationToken = default);

	/// <summary>
	///     Handle the GetAlertByIdQuery.
	/// </summary>
	/// <remarks>
	///     This method handles the GetAlertByIdQuery. It returns the alert for the given Id.
	/// </remarks>
	/// <param name="query">The GetAlertByIdQuery query</param>
	/// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
	/// <returns>The Alert object if found, or null otherwise</returns>
	Task<Alert?> Handle(GetAlertByIdQuery query, CancellationToken cancellationToken = default);

	/// <summary>
	///     Handle the GetAllAlertsByVehicleIdQuery.
	/// </summary>
	/// <remarks>
	///     This method handles the GetAllAlertsByVehicleIdQuery. It returns all the alerts for the given vehicle ID.
	/// </remarks>
	/// <param name="query">The GetAllAlertsByVehicleIdQuery query</param>
	/// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
	/// <returns>An IEnumerable containing the Alert objects</returns>
	Task<IEnumerable<Alert>> Handle(GetAllAlertsByVehicleIdQuery query, CancellationToken cancellationToken = default);
}
