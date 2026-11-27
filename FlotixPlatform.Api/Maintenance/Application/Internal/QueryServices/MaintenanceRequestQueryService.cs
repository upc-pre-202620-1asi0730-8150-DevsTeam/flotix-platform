using FlotixPlatform.Api.Maintenance.Domain.Model.Aggregates;
using FlotixPlatform.Api.Maintenance.Application.Services;
using FlotixPlatform.Api.Maintenance.Domain.Model.Queries;
using FlotixPlatform.Api.Maintenance.Domain.Repositories;

namespace FlotixPlatform.Api.Maintenance.Application.Internal.QueryServices;

/// <summary>
///     Application service for querying maintenance requests.
/// </summary>
/// <remarks>
///     This service acts as the query handler for retrieving maintenance requests.
///     All queries delegate to the repository layer for data access.
/// </remarks>
/// <param name="maintenanceRequestRepository">Repository for accessing maintenance request data.</param>
public class MaintenanceRequestQueryService(IMaintenanceRequestRepository maintenanceRequestRepository)
    : IMaintenanceRequestQueryService
{
    /// <inheritdoc />
    public async Task<IEnumerable<MaintenanceRequest>> Handle(GetAllMaintenanceRequestsQuery query,
        CancellationToken cancellationToken = default) =>
        await maintenanceRequestRepository.ListAsync(cancellationToken);

    /// <inheritdoc />
    public async Task<MaintenanceRequest?> Handle(GetMaintenanceRequestByIdQuery query,
        CancellationToken cancellationToken = default) =>
        await maintenanceRequestRepository.FindByIdAsync(query.Id, cancellationToken);

    /// <inheritdoc />
    public async Task<IEnumerable<MaintenanceRequest>> Handle(GetAllMaintenanceRequestsByVehicleIdQuery query,
        CancellationToken cancellationToken = default) =>
        await maintenanceRequestRepository.FindByVehicleIdAsync(query.VehicleId, cancellationToken);

    /// <inheritdoc />
    public async Task<IEnumerable<MaintenanceRequest>> Handle(GetAllMaintenanceRequestsByMechanicIdQuery query,
        CancellationToken cancellationToken = default) =>
        await maintenanceRequestRepository.FindByMechanicIdAsync(query.MechanicId, cancellationToken);
}
