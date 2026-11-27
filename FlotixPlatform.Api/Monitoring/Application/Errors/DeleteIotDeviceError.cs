namespace FlotixPlatform.Api.Monitoring.Application.Errors;

/// <summary>
/// Represents errors that can occur when deleting an IoT device.
/// </summary>
public enum DeleteIotDeviceError
{
	/// <summary>
	/// The IoT device to delete does not exist.
	/// </summary>
	IotDeviceNotFound,

	/// <summary>
	/// The IoT device is referenced by other records and cannot be deleted.
	/// </summary>
	IotDeviceHasDependents,

	/// <summary>
	/// An unexpected error occurred during the operation.
	/// </summary>
	UnexpectedError
}
