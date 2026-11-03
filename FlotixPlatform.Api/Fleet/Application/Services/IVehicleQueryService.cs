using FlotixPlatform.Api.Fleet.Domain.Model.Aggregates;
using FlotixPlatform.Api.Fleet.Domain.Model.Queries;
using FlotixPlatform.API.Fleet.Domain.Model.Queries;
using FlotixPlatform.Api.Fleet.Domain.Queries;

namespace FlotixPlatform.Api.Fleet.Application.Services;

/// <summary>
///     Query service interface for vehicle operations.
/// </summary>
/// <remarks>
///     This interface defines the contract for querying vehicles by various filters.
/// </remarks>
public interface IVehicleQueryService
{
	/// <summary>
	///     Handle the GetAllVehiclesQuery.
	/// </summary>
	/// <remarks>
	///     This method handles the GetAllVehiclesQuery. It returns all the vehicles.
	/// </remarks>
	/// <param name="query">The GetAllVehiclesQuery query</param>
	/// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
	/// <returns>An IEnumerable containing the Vehicle objects</returns>
	Task<IEnumerable<Vehicle>> Handle(GetAllVehiclesQuery query, CancellationToken cancellationToken = default);

	/// <summary>
	///     Handle the GetVehicleByIdQuery.
	/// </summary>
	/// <remarks>
	///     This method handles the GetVehicleByIdQuery. It returns the vehicle for the given Id.
	/// </remarks>
	/// <param name="query">The GetVehicleByIdQuery query</param>
	/// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
	/// <returns>The Vehicle object if found, or null otherwise</returns>
	Task<Vehicle?> Handle(GetVehicleByIdQuery query, CancellationToken cancellationToken = default);

	/// <summary>
	///     Handle the GetAllVehiclesByOwnerIdQuery.
	/// </summary>
	/// <remarks>
	///     This method handles the GetAllVehiclesByOwnerIdQuery. It returns all the vehicles for the given owner ID.
	/// </remarks>
	/// <param name="query">The GetAllVehiclesByOwnerIdQuery query</param>
	/// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
	/// <returns>An IEnumerable containing the Vehicle objects</returns>
	Task<IEnumerable<Vehicle>> Handle(GetAllVehiclesByOwnerIdQuery query, CancellationToken cancellationToken = default);

	/// <summary>
	///     Handle the GetAllVehiclesByDriverIdQuery.
	/// </summary>
	/// <remarks>
	///     This method handles the GetAllVehiclesByDriverIdQuery. It returns all the vehicles for the given driver ID.
	/// </remarks>
	/// <param name="query">The GetAllVehiclesByDriverIdQuery query</param>
	/// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
	/// <returns>An IEnumerable containing the Vehicle objects</returns>
	Task<IEnumerable<Vehicle>> Handle(GetAllVehiclesByDriverIdQuery query, CancellationToken cancellationToken = default);
}