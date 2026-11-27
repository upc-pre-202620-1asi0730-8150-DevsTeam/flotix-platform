namespace FlotixPlatform.Api.Fuel.Application.Errors;

/// <summary>
/// Represents errors that can occur when deleting a fuel record.
/// </summary>
public enum DeleteFuelRecordError
{
	/// <summary>
	/// The fuel record to delete does not exist.
	/// </summary>
	FuelRecordNotFound,

	/// <summary>
	/// An unexpected error occurred during the operation.
	/// </summary>
	UnexpectedError
}
