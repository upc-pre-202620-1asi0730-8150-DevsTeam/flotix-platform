using FlotixPlatform.API.Maintenance.Domain.Model.Aggregates;
using FlotixPlatform.API.Maintenance.Domain.Model.Queries;

namespace FlotixPlatform.API.Maintenance.Application.Services;

/// <summary>
///     Query service interface for maintenance request operations.
/// </summary>
/// <remarks>
///     This interface defines the contract for querying maintenance requests by various filters.
/// </remarks>
public interface IMaintenanceRequestQueryService
{
	/// <summary>
	///     Handle the GetAllMaintenanceRequestsQuery.
	/// </summary>
	/// <remarks>
	///     This method handles the GetAllMaintenanceRequestsQuery. It returns all the maintenance requests.
	/// </remarks>
	/// <param name="query">The GetAllMaintenanceRequestsQuery query</param>
	/// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
	/// <returns>An IEnumerable containing the MaintenanceRequest objects</returns>
	Task<IEnumerable<MaintenanceRequest>> Handle(GetAllMaintenanceRequestsQuery query, CancellationToken cancellationToken = default);

	/// <summary>
	///     Handle the GetMaintenanceRequestByIdQuery.
	/// </summary>
	/// <remarks>
	///     This method handles the GetMaintenanceRequestByIdQuery. It returns the maintenance request for the given Id.
	/// </remarks>
	/// <param name="query">The GetMaintenanceRequestByIdQuery query</param>
	/// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
	/// <returns>The MaintenanceRequest object if found, or null otherwise</returns>
	Task<MaintenanceRequest?> Handle(GetMaintenanceRequestByIdQuery query, CancellationToken cancellationToken = default);

	/// <summary>
	///     Handle the GetAllMaintenanceRequestsByVehicleIdQuery.
	/// </summary>
	/// <remarks>
	///     This method handles the GetAllMaintenanceRequestsByVehicleIdQuery. It returns all the maintenance requests for the given vehicle ID.
	/// </remarks>
	/// <param name="query">The GetAllMaintenanceRequestsByVehicleIdQuery query</param>
	/// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
	/// <returns>An IEnumerable containing the MaintenanceRequest objects</returns>
	Task<IEnumerable<MaintenanceRequest>> Handle(GetAllMaintenanceRequestsByVehicleIdQuery query, CancellationToken cancellationToken = default);

	/// <summary>
	///     Handle the GetAllMaintenanceRequestsByMechanicIdQuery.
	/// </summary>
	/// <remarks>
	///     This method handles the GetAllMaintenanceRequestsByMechanicIdQuery. It returns all the maintenance requests for the given mechanic ID.
	/// </remarks>
	/// <param name="query">The GetAllMaintenanceRequestsByMechanicIdQuery query</param>
	/// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
	/// <returns>An IEnumerable containing the MaintenanceRequest objects</returns>
	Task<IEnumerable<MaintenanceRequest>> Handle(GetAllMaintenanceRequestsByMechanicIdQuery query, CancellationToken cancellationToken = default);
}
