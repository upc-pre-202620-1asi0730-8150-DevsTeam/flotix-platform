using FlotixPlatform.Api.Monitoring.Domain.Model.Aggregates;
using FlotixPlatform.Api.Monitoring.Domain.Model.Queries;

namespace FlotixPlatform.Api.Monitoring.Application.Services;

/// <summary>
///     Query service interface for IoT device operations.
/// </summary>
/// <remarks>
///     This interface defines the contract for querying IoT devices by various filters.
/// </remarks>
public interface IIotDeviceQueryService
{
	/// <summary>
	///     Handle the GetAllIotDevicesQuery.
	/// </summary>
	/// <remarks>
	///     This method handles the GetAllIotDevicesQuery. It returns all the IoT devices.
	/// </remarks>
	/// <param name="query">The GetAllIotDevicesQuery query</param>
	/// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
	/// <returns>An IEnumerable containing the IotDevice objects</returns>
	Task<IEnumerable<IotDevice>> Handle(GetAllIotDevicesQuery query, CancellationToken cancellationToken = default);

	/// <summary>
	///     Handle the GetIotDeviceByIdQuery.
	/// </summary>
	/// <remarks>
	///     This method handles the GetIotDeviceByIdQuery. It returns the IoT device for the given Id.
	/// </remarks>
	/// <param name="query">The GetIotDeviceByIdQuery query</param>
	/// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
	/// <returns>The IotDevice object if found, or null otherwise</returns>
	Task<IotDevice?> Handle(GetIotDeviceByIdQuery query, CancellationToken cancellationToken = default);

	/// <summary>
	///     Handle the GetAllIotDevicesByVehicleIdQuery.
	/// </summary>
	/// <remarks>
	///     This method handles the GetAllIotDevicesByVehicleIdQuery. It returns all the IoT devices for the given vehicle ID.
	/// </remarks>
	/// <param name="query">The GetAllIotDevicesByVehicleIdQuery query</param>
	/// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
	/// <returns>An IEnumerable containing the IotDevice objects</returns>
	Task<IEnumerable<IotDevice>> Handle(GetAllIotDevicesByVehicleIdQuery query, CancellationToken cancellationToken = default);
}
