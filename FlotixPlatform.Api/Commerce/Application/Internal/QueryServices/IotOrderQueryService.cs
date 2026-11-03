using FlotixPlatform.API.Commerce.Domain.Model.Aggregates;
using FlotixPlatform.API.Commerce.Application.Services;
using FlotixPlatform.API.Commerce.Domain.Model.Queries;
using FlotixPlatform.API.Commerce.Domain.Repositories;

namespace FlotixPlatform.API.Commerce.Application.Internal.QueryServices;

/// <summary>
///     Application service for querying IoT orders.
/// </summary>
/// <remarks>
///     This service acts as the query handler for retrieving IoT orders.
///     All queries delegate to the repository layer for data access.
/// </remarks>
/// <param name="iotOrderRepository">Repository for accessing IoT order data.</param>
public class IotOrderQueryService(IIotOrderRepository iotOrderRepository)
    : IIotOrderQueryService
{
    /// <inheritdoc />
    public async Task<IEnumerable<IotOrder>> Handle(GetAllIotOrdersQuery query,
        CancellationToken cancellationToken = default) =>
        await iotOrderRepository.ListAsync(cancellationToken);

    /// <inheritdoc />
    public async Task<IotOrder?> Handle(GetIotOrderByIdQuery query,
        CancellationToken cancellationToken = default) =>
        await iotOrderRepository.FindByIdAsync(query.Id, cancellationToken);

    /// <inheritdoc />
    public async Task<IEnumerable<IotOrder>> Handle(GetAllIotOrdersByOwnerIdQuery query,
        CancellationToken cancellationToken = default) =>
        await iotOrderRepository.FindByOwnerIdAsync(query.OwnerId, cancellationToken);
}
