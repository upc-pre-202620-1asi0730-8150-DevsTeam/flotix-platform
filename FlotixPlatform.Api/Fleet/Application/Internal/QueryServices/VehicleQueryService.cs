using FlotixPlatform.Api.Fleet.Domain.Model.Aggregates;
using FlotixPlatform.Api.Fleet.Application.Services;
using FlotixPlatform.Api.Fleet.Domain.Model.Queries;
using FlotixPlatform.Api.Fleet.Domain.Repositories;

namespace FlotixPlatform.API.Fleet.Application.Internal.QueryServices;

/// <summary>
///     Application service for querying vehicles.
/// </summary>
/// <remarks>
///     This service acts as the query handler for retrieving vehicles.
///     All queries delegate to the repository layer for data access.
/// </remarks>
/// <param name="vehicleRepository">Repository for accessing vehicle data.</param>
public class VehicleQueryService(IVehicleRepository vehicleRepository)
    : IVehicleQueryService
{
    /// <inheritdoc />
    public async Task<IEnumerable<Vehicle>> Handle(GetAllVehiclesQuery query,
        CancellationToken cancellationToken = default) =>
        await vehicleRepository.ListAsync(cancellationToken);

    /// <inheritdoc />
    public async Task<Vehicle?> Handle(GetVehicleByIdQuery query,
        CancellationToken cancellationToken = default) =>
        await vehicleRepository.FindByIdAsync(query.Id, cancellationToken);

    /// <inheritdoc />
    public async Task<IEnumerable<Vehicle>> Handle(GetAllVehiclesByOwnerIdQuery query,
        CancellationToken cancellationToken = default) =>
        await vehicleRepository.FindByOwnerIdAsync(query.OwnerId, cancellationToken);

    /// <inheritdoc />
    public async Task<IEnumerable<Vehicle>> Handle(GetAllVehiclesByDriverIdQuery query,
        CancellationToken cancellationToken = default) =>
        await vehicleRepository.FindByDriverIdAsync(query.DriverId, cancellationToken);
}