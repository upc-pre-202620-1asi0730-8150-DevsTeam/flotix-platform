using FlotixPlatform.Api.Fuel.Domain.Model.Aggregates;
using FlotixPlatform.Api.Fuel.Domain.Repositories;
using FlotixPlatform.Api.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using FlotixPlatform.Api.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using Microsoft.EntityFrameworkCore;

namespace FlotixPlatform.Api.Fuel.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

/// <summary>
///     Entity Framework repository for fuel record persistence.
/// </summary>
/// <remarks>
///     This repository extends the generic BaseRepository with fuel record-specific query methods.
/// </remarks>
/// <param name="context">The EF Core database context.</param>
public class FuelRecordRepository(AppDbContext context)
    : BaseRepository<FuelRecord>(context), IFuelRecordRepository
{
    /// <inheritdoc />
    public async Task<IEnumerable<FuelRecord>> FindByVehicleIdAsync(Guid vehicleId, CancellationToken cancellationToken = default)
    {
        return await Context.Set<FuelRecord>()
            .Where(f => f.VehicleId == vehicleId)
            .ToListAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<FuelRecord>> FindByDriverIdAsync(Guid driverId, CancellationToken cancellationToken = default)
    {
        return await Context.Set<FuelRecord>()
            .Where(f => f.DriverId == driverId)
            .ToListAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<FuelRecord>> FindByVehicleIdsAndRecordedDateRangeAsync(IReadOnlyCollection<Guid> vehicleIds,
        DateTimeOffset from, DateTimeOffset to, CancellationToken cancellationToken = default)
    {
        return await Context.Set<FuelRecord>()
            .Where(f => vehicleIds.Contains(f.VehicleId) && f.RecordedDate >= from && f.RecordedDate < to)
            .ToListAsync(cancellationToken);
    }
}
