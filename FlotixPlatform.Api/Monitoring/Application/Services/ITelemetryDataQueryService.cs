using FlotixPlatform.Api.Monitoring.Domain.Model.Aggregates;
using FlotixPlatform.Api.Monitoring.Domain.Model.Queries;

namespace FlotixPlatform.Api.Monitoring.Application.Services;

/// <summary>
///     Query service interface for telemetry record operations.
/// </summary>
/// <remarks>
///     This interface defines the contract for querying telemetry records by various filters.
/// </remarks>
public interface ITelemetryDataQueryService
{
	/// <summary>
	///     Handle the GetAllTelemetryDataQuery.
	/// </summary>
	/// <remarks>
	///     This method handles the GetAllTelemetryDataQuery. It returns all the telemetry records.
	/// </remarks>
	/// <param name="query">The GetAllTelemetryDataQuery query</param>
	/// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
	/// <returns>An IEnumerable containing the TelemetryData objects</returns>
	Task<IEnumerable<TelemetryData>> Handle(GetAllTelemetryDataQuery query, CancellationToken cancellationToken = default);

	/// <summary>
	///     Handle the GetTelemetryDataByIdQuery.
	/// </summary>
	/// <remarks>
	///     This method handles the GetTelemetryDataByIdQuery. It returns the telemetry record for the given Id.
	/// </remarks>
	/// <param name="query">The GetTelemetryDataByIdQuery query</param>
	/// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
	/// <returns>The TelemetryData object if found, or null otherwise</returns>
	Task<TelemetryData?> Handle(GetTelemetryDataByIdQuery query, CancellationToken cancellationToken = default);

	/// <summary>
	///     Handle the GetAllTelemetryDataByDeviceIdQuery.
	/// </summary>
	/// <remarks>
	///     This method handles the GetAllTelemetryDataByDeviceIdQuery. It returns all the telemetry records for the given device ID.
	/// </remarks>
	/// <param name="query">The GetAllTelemetryDataByDeviceIdQuery query</param>
	/// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
	/// <returns>An IEnumerable containing the TelemetryData objects</returns>
	Task<IEnumerable<TelemetryData>> Handle(GetAllTelemetryDataByDeviceIdQuery query, CancellationToken cancellationToken = default);
}
