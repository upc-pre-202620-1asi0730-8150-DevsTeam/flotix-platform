namespace FlotixPlatform.Api.Fuel.Application.Errors;

/// <summary>
/// Represents errors that can occur when creating a fuel record.
/// </summary>
public enum CreateFuelRecordError
{
	/// <summary>
	/// The provided fuel record data is invalid.
	/// </summary>
	InvalidFuelRecord,

	/// <summary>
	/// A resource referenced by the command does not exist.
	/// </summary>
	ReferencedResourceNotFound,

	/// <summary>
	/// An unexpected error occurred during the operation.
	/// </summary>
	UnexpectedError
}
