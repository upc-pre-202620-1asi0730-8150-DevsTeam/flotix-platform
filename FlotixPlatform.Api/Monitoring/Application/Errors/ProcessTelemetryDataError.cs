namespace FlotixPlatform.Api.Monitoring.Application.Errors;

/// <summary>
/// Represents errors that can occur when processing a received telemetry packet.
/// </summary>
public enum ProcessTelemetryDataError
{
	/// <summary>
	/// The packet comes from a device that is not linked to a vehicle and was discarded.
	/// </summary>
	IotDeviceNotFound,

	/// <summary>
	/// The packet does not follow the expected schema and was discarded.
	/// </summary>
	InvalidTelemetryData,

	/// <summary>
	/// An unexpected error occurred during the operation.
	/// </summary>
	UnexpectedError
}
