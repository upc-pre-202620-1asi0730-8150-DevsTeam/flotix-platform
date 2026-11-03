using FlotixPlatform.API.Commerce.Domain.Model.Aggregates;
using FlotixPlatform.API.Commerce.Domain.Model.Queries;

namespace FlotixPlatform.API.Commerce.Application.Services;

/// <summary>
///     Query service interface for IoT order operations.
/// </summary>
/// <remarks>
///     This interface defines the contract for querying IoT orders by various filters.
/// </remarks>
public interface IIotOrderQueryService
{
	/// <summary>
	///     Handle the GetAllIotOrdersQuery.
	/// </summary>
	/// <remarks>
	///     This method handles the GetAllIotOrdersQuery. It returns all the IoT orders.
	/// </remarks>
	/// <param name="query">The GetAllIotOrdersQuery query</param>
	/// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
	/// <returns>An IEnumerable containing the IotOrder objects</returns>
	Task<IEnumerable<IotOrder>> Handle(GetAllIotOrdersQuery query, CancellationToken cancellationToken = default);

	/// <summary>
	///     Handle the GetIotOrderByIdQuery.
	/// </summary>
	/// <remarks>
	///     This method handles the GetIotOrderByIdQuery. It returns the IoT order for the given Id.
	/// </remarks>
	/// <param name="query">The GetIotOrderByIdQuery query</param>
	/// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
	/// <returns>The IotOrder object if found, or null otherwise</returns>
	Task<IotOrder?> Handle(GetIotOrderByIdQuery query, CancellationToken cancellationToken = default);

	/// <summary>
	///     Handle the GetAllIotOrdersByOwnerIdQuery.
	/// </summary>
	/// <remarks>
	///     This method handles the GetAllIotOrdersByOwnerIdQuery. It returns all the IoT orders for the given owner ID.
	/// </remarks>
	/// <param name="query">The GetAllIotOrdersByOwnerIdQuery query</param>
	/// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
	/// <returns>An IEnumerable containing the IotOrder objects</returns>
	Task<IEnumerable<IotOrder>> Handle(GetAllIotOrdersByOwnerIdQuery query, CancellationToken cancellationToken = default);
}
