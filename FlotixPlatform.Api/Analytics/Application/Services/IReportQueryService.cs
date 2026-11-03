using FlotixPlatform.API.Analytics.Domain.Model.Aggregates;
using FlotixPlatform.API.Analytics.Domain.Model.Queries;

namespace FlotixPlatform.API.Analytics.Application.Services;

/// <summary>
///     Query service interface for report operations.
/// </summary>
/// <remarks>
///     This interface defines the contract for querying reports by various filters.
/// </remarks>
public interface IReportQueryService
{
	/// <summary>
	///     Handle the GetAllReportsQuery.
	/// </summary>
	/// <remarks>
	///     This method handles the GetAllReportsQuery. It returns all the reports.
	/// </remarks>
	/// <param name="query">The GetAllReportsQuery query</param>
	/// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
	/// <returns>An IEnumerable containing the Report objects</returns>
	Task<IEnumerable<Report>> Handle(GetAllReportsQuery query, CancellationToken cancellationToken = default);

	/// <summary>
	///     Handle the GetReportByIdQuery.
	/// </summary>
	/// <remarks>
	///     This method handles the GetReportByIdQuery. It returns the report for the given Id.
	/// </remarks>
	/// <param name="query">The GetReportByIdQuery query</param>
	/// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
	/// <returns>The Report object if found, or null otherwise</returns>
	Task<Report?> Handle(GetReportByIdQuery query, CancellationToken cancellationToken = default);

	/// <summary>
	///     Handle the GetAllReportsByOwnerIdQuery.
	/// </summary>
	/// <remarks>
	///     This method handles the GetAllReportsByOwnerIdQuery. It returns all the reports for the given owner ID.
	/// </remarks>
	/// <param name="query">The GetAllReportsByOwnerIdQuery query</param>
	/// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
	/// <returns>An IEnumerable containing the Report objects</returns>
	Task<IEnumerable<Report>> Handle(GetAllReportsByOwnerIdQuery query, CancellationToken cancellationToken = default);
}
