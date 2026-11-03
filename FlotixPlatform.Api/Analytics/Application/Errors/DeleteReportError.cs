namespace FlotixPlatform.Api.Analytics.Application.Errors;

/// <summary>
/// Represents errors that can occur when deleting a report.
/// </summary>
public enum DeleteReportError
{
	/// <summary>
	/// The report to delete does not exist.
	/// </summary>
	ReportNotFound,

	/// <summary>
	/// An unexpected error occurred during the operation.
	/// </summary>
	UnexpectedError
}
