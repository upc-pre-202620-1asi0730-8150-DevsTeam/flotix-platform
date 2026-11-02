using FlotixPlatform.API.Maintenance.Domain.Model.Aggregates;
using FlotixPlatform.API.Maintenance.Domain.Repositories;
using FlotixPlatform.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using FlotixPlatform.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using Microsoft.EntityFrameworkCore;

namespace FlotixPlatform.API.Maintenance.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

/// <summary>
///     Entity Framework repository for maintenance request persistence.
/// </summary>
/// <remarks>
///     This repository extends the generic BaseRepository with maintenance request-specific query methods.
/// </remarks>
/// <param name="context">The EF Core database context.</param>
public class MaintenanceRequestRepository(AppDbContext context)
    : BaseRepository<MaintenanceRequest>(context), IMaintenanceRequestRepository
{
    /// <inheritdoc />
    public async Task<IEnumerable<MaintenanceRequest>> FindByVehicleIdAsync(Guid vehicleId, CancellationToken cancellationToken = default)
    {
        return await Context.Set<MaintenanceRequest>()
            .Where(m => m.VehicleId == vehicleId)
            .ToListAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<MaintenanceRequest>> FindByMechanicIdAsync(Guid mechanicId, CancellationToken cancellationToken = default)
    {
        return await Context.Set<MaintenanceRequest>()
            .Where(m => m.MechanicId == mechanicId)
            .ToListAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<MaintenanceRequest>> FindByVehicleIdsAndRequestDateRangeAsync(IReadOnlyCollection<Guid> vehicleIds,
        DateTimeOffset from, DateTimeOffset to, CancellationToken cancellationToken = default)
    {
        return await Context.Set<MaintenanceRequest>()
            .Where(m => vehicleIds.Contains(m.VehicleId) && m.RequestDate >= from && m.RequestDate < to)
            .ToListAsync(cancellationToken);
    }
}
