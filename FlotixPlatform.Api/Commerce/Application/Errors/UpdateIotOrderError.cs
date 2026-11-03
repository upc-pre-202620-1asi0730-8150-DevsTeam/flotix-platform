namespace FlotixPlatform.API.Commerce.Application.Errors;

/// <summary>
/// Represents errors that can occur when updating an IoT order.
/// </summary>
public enum UpdateIotOrderError
{
	/// <summary>
	/// The IoT order to update does not exist.
	/// </summary>
	IotOrderNotFound,

	/// <summary>
	/// The provided IoT order data is invalid.
	/// </summary>
	InvalidIotOrder,

	/// <summary>
	/// A delivered or cancelled order cannot change its status.
	/// </summary>
	IotOrderAlreadyClosed,

	/// <summary>
	/// An unexpected error occurred during the operation.
	/// </summary>
	UnexpectedError
}
