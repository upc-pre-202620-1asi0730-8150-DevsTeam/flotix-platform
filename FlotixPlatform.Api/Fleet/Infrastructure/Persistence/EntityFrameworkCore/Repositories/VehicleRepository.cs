using FlotixPlatform.Api.Fleet.Domain.Model.Aggregates;
using FlotixPlatform.Api.Fleet.Domain.Model.ValueObjects;
using FlotixPlatform.Api.Fleet.Domain.Repositories;
using FlotixPlatform.Api.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using FlotixPlatform.Api.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using Microsoft.EntityFrameworkCore;

namespace FlotixPlatform.Api.Fleet.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

/// <summary>
///     Entity Framework repository for vehicle persistence.
/// </summary>
/// <remarks>
///     This repository extends the generic BaseRepository with vehicle-specific query methods.
/// </remarks>
/// <param name="context">The EF Core database context.</param>
public class VehicleRepository(AppDbContext context)
    : BaseRepository<Vehicle>(context), IVehicleRepository
{
    /// <inheritdoc />
    public async Task<IEnumerable<Vehicle>> FindByOwnerIdAsync(Guid ownerId, CancellationToken cancellationToken = default)
    {
        return await Context.Set<Vehicle>()
            .Where(v => v.OwnerId == ownerId)
            .ToListAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<Vehicle>> FindByDriverIdAsync(Guid driverId, CancellationToken cancellationToken = default)
    {
        return await Context.Set<Vehicle>()
            .Where(v => v.DriverId == driverId)
            .ToListAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task<bool> ExistsByLicensePlateAsync(LicensePlate licensePlate, CancellationToken cancellationToken = default)
    {
        return await Context.Set<Vehicle>().AnyAsync(v => v.LicensePlate == licensePlate, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<Vehicle?> FindByLicensePlateAsync(LicensePlate licensePlate, CancellationToken cancellationToken = default)
    {
        return await Context.Set<Vehicle>().FirstOrDefaultAsync(v => v.LicensePlate == licensePlate, cancellationToken);
    }
}