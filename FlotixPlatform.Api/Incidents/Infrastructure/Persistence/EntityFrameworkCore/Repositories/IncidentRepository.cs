using FlotixPlatform.Api.Incidents.Domain.Model.Aggregates;
using FlotixPlatform.Api.Incidents.Domain.Repositories;
using FlotixPlatform.Api.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using FlotixPlatform.Api.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using Microsoft.EntityFrameworkCore;

namespace FlotixPlatform.Api.Incidents.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

/// <summary>
///     Entity Framework repository for incident persistence.
/// </summary>
/// <remarks>
///     This repository extends the generic BaseRepository with incident-specific query methods.
/// </remarks>
/// <param name="context">The EF Core database context.</param>
public class IncidentRepository(AppDbContext context)
    : BaseRepository<Incident>(context), IIncidentRepository
{
    /// <inheritdoc />
    public async Task<IEnumerable<Incident>> FindByVehicleIdAsync(Guid vehicleId, CancellationToken cancellationToken = default)
    {
        return await Context.Set<Incident>()
            .Where(i => i.VehicleId == vehicleId)
            .ToListAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<Incident>> FindByDriverIdAsync(Guid driverId, CancellationToken cancellationToken = default)
    {
        return await Context.Set<Incident>()
            .Where(i => i.DriverId == driverId)
            .ToListAsync(cancellationToken);
    }
}
