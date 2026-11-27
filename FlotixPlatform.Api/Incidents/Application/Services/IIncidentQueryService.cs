using FlotixPlatform.Api.Incidents.Domain.Model.Aggregates;
using FlotixPlatform.Api.Incidents.Domain.Model.Queries;

namespace FlotixPlatform.Api.Incidents.Application.Services;

/// <summary>
///     Query service interface for incident operations.
/// </summary>
/// <remarks>
///     This interface defines the contract for querying incidents by various filters.
/// </remarks>
public interface IIncidentQueryService
{
	/// <summary>
	///     Handle the GetAllIncidentsQuery.
	/// </summary>
	/// <remarks>
	///     This method handles the GetAllIncidentsQuery. It returns all the incidents.
	/// </remarks>
	/// <param name="query">The GetAllIncidentsQuery query</param>
	/// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
	/// <returns>An IEnumerable containing the Incident objects</returns>
	Task<IEnumerable<Incident>> Handle(GetAllIncidentsQuery query, CancellationToken cancellationToken = default);

	/// <summary>
	///     Handle the GetIncidentByIdQuery.
	/// </summary>
	/// <remarks>
	///     This method handles the GetIncidentByIdQuery. It returns the incident for the given Id.
	/// </remarks>
	/// <param name="query">The GetIncidentByIdQuery query</param>
	/// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
	/// <returns>The Incident object if found, or null otherwise</returns>
	Task<Incident?> Handle(GetIncidentByIdQuery query, CancellationToken cancellationToken = default);

	/// <summary>
	///     Handle the GetAllIncidentsByVehicleIdQuery.
	/// </summary>
	/// <remarks>
	///     This method handles the GetAllIncidentsByVehicleIdQuery. It returns all the incidents for the given vehicle ID.
	/// </remarks>
	/// <param name="query">The GetAllIncidentsByVehicleIdQuery query</param>
	/// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
	/// <returns>An IEnumerable containing the Incident objects</returns>
	Task<IEnumerable<Incident>> Handle(GetAllIncidentsByVehicleIdQuery query, CancellationToken cancellationToken = default);

	/// <summary>
	///     Handle the GetAllIncidentsByDriverIdQuery.
	/// </summary>
	/// <remarks>
	///     This method handles the GetAllIncidentsByDriverIdQuery. It returns all the incidents for the given driver ID.
	/// </remarks>
	/// <param name="query">The GetAllIncidentsByDriverIdQuery query</param>
	/// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
	/// <returns>An IEnumerable containing the Incident objects</returns>
	Task<IEnumerable<Incident>> Handle(GetAllIncidentsByDriverIdQuery query, CancellationToken cancellationToken = default);
}
