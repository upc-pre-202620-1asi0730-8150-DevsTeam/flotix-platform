namespace FlotixPlatform.Api.Monitoring.Application.Errors;

/// <summary>
/// Represents errors that can occur when updating an IoT device.
/// </summary>
public enum UpdateIotDeviceError
{
	/// <summary>
	/// The IoT device to update does not exist.
	/// </summary>
	IotDeviceNotFound,

	/// <summary>
	/// The provided IoT device data is invalid.
	/// </summary>
	InvalidIotDevice,

	/// <summary>
	/// An IoT device with the same vehicle ID already exists.
	/// </summary>
	DuplicateIotDeviceVehicleId,

	/// <summary>
	/// A resource referenced by the command does not exist.
	/// </summary>
	ReferencedResourceNotFound,

	/// <summary>
	/// An unexpected error occurred during the operation.
	/// </summary>
	UnexpectedError
}
