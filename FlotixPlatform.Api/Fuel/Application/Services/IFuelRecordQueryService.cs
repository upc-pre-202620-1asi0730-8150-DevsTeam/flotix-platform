using FlotixPlatform.API.Fuel.Domain.Model.Aggregates;
using FlotixPlatform.API.Fuel.Domain.Model.Queries;

namespace FlotixPlatform.Api.Fuel.Application.Services;

/// <summary>
///     Query service interface for fuel record operations.
/// </summary>
/// <remarks>
///     This interface defines the contract for querying fuel records by various filters.
/// </remarks>
public interface IFuelRecordQueryService
{
	/// <summary>
	///     Handle the GetAllFuelRecordsQuery.
	/// </summary>
	/// <remarks>
	///     This method handles the GetAllFuelRecordsQuery. It returns all the fuel records.
	/// </remarks>
	/// <param name="query">The GetAllFuelRecordsQuery query</param>
	/// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
	/// <returns>An IEnumerable containing the FuelRecord objects</returns>
	Task<IEnumerable<FuelRecord>> Handle(GetAllFuelRecordsQuery query, CancellationToken cancellationToken = default);

	/// <summary>
	///     Handle the GetFuelRecordByIdQuery.
	/// </summary>
	/// <remarks>
	///     This method handles the GetFuelRecordByIdQuery. It returns the fuel record for the given Id.
	/// </remarks>
	/// <param name="query">The GetFuelRecordByIdQuery query</param>
	/// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
	/// <returns>The FuelRecord object if found, or null otherwise</returns>
	Task<FuelRecord?> Handle(GetFuelRecordByIdQuery query, CancellationToken cancellationToken = default);

	/// <summary>
	///     Handle the GetAllFuelRecordsByVehicleIdQuery.
	/// </summary>
	/// <remarks>
	///     This method handles the GetAllFuelRecordsByVehicleIdQuery. It returns all the fuel records for the given vehicle ID.
	/// </remarks>
	/// <param name="query">The GetAllFuelRecordsByVehicleIdQuery query</param>
	/// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
	/// <returns>An IEnumerable containing the FuelRecord objects</returns>
	Task<IEnumerable<FuelRecord>> Handle(GetAllFuelRecordsByVehicleIdQuery query, CancellationToken cancellationToken = default);

	/// <summary>
	///     Handle the GetAllFuelRecordsByDriverIdQuery.
	/// </summary>
	/// <remarks>
	///     This method handles the GetAllFuelRecordsByDriverIdQuery. It returns all the fuel records for the given driver ID.
	/// </remarks>
	/// <param name="query">The GetAllFuelRecordsByDriverIdQuery query</param>
	/// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
	/// <returns>An IEnumerable containing the FuelRecord objects</returns>
	Task<IEnumerable<FuelRecord>> Handle(GetAllFuelRecordsByDriverIdQuery query, CancellationToken cancellationToken = default);
}
