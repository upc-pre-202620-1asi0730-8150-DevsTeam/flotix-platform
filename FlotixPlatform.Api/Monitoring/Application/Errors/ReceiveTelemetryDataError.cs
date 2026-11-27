namespace FlotixPlatform.Api.Monitoring.Application.Errors;

/// <summary>
/// Represents errors that can occur when receiving a telemetry packet.
/// </summary>
public enum ReceiveTelemetryDataError
{
	/// <summary>
	/// The packet does not follow the expected schema and was discarded.
	/// </summary>
	InvalidTelemetryData,

	/// <summary>
	/// An unexpected error occurred during the operation.
	/// </summary>
	UnexpectedError
}
