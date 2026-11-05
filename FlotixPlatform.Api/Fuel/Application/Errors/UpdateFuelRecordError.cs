namespace FlotixPlatform.Api.Fuel.Application.Errors;

/// <summary>
/// Represents errors that can occur when updating a fuel record.
/// </summary>
public enum UpdateFuelRecordError
{
	/// <summary>
	/// The fuel record to update does not exist.
	/// </summary>
	FuelRecordNotFound,

	/// <summary>
	/// The provided fuel record data is invalid.
	/// </summary>
	InvalidFuelRecord,

	/// <summary>
	/// An unexpected error occurred during the operation.
	/// </summary>
	UnexpectedError
}
