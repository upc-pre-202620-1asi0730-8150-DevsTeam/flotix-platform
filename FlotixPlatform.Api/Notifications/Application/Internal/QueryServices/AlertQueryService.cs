using FlotixPlatform.Api.Notifications.Domain.Model.Aggregates;
using FlotixPlatform.Api.Notifications.Application.Services;
using FlotixPlatform.Api.Notifications.Domain.Model.Queries;
using FlotixPlatform.Api.Notifications.Domain.Repositories;

namespace FlotixPlatform.Api.Notifications.Application.Internal.QueryServices;

/// <summary>
///     Application service for querying alerts.
/// </summary>
/// <remarks>
///     This service acts as the query handler for retrieving alerts.
///     All queries delegate to the repository layer for data access.
/// </remarks>
/// <param name="alertRepository">Repository for accessing alert data.</param>
public class AlertQueryService(IAlertRepository alertRepository)
    : IAlertQueryService
{
    /// <inheritdoc />
    public async Task<IEnumerable<Alert>> Handle(GetAllAlertsQuery query,
        CancellationToken cancellationToken = default) =>
        await alertRepository.ListAsync(cancellationToken);

    /// <inheritdoc />
    public async Task<Alert?> Handle(GetAlertByIdQuery query,
        CancellationToken cancellationToken = default) =>
        await alertRepository.FindByIdAsync(query.Id, cancellationToken);

    /// <inheritdoc />
    public async Task<IEnumerable<Alert>> Handle(GetAllAlertsByVehicleIdQuery query,
        CancellationToken cancellationToken = default) =>
        await alertRepository.FindByVehicleIdAsync(query.VehicleId, cancellationToken);
}
