namespace FlotixPlatform.Api.Analytics.Application.Errors;

/// <summary>
/// Represents errors that can occur when generating a report.
/// </summary>
public enum GenerateReportError
{
	/// <summary>
	/// The report type is not supported or the period is invalid.
	/// </summary>
	InvalidReport,

	/// <summary>
	/// The owner of the report does not exist.
	/// </summary>
	ReferencedResourceNotFound,

	/// <summary>
	/// An unexpected error occurred during the operation.
	/// </summary>
	UnexpectedError
}
