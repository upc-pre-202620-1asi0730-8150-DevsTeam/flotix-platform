using FlotixPlatform.Api.Notifications.Domain.Model.Aggregates;
using FlotixPlatform.Api.Notifications.Domain.Repositories;
using FlotixPlatform.Api.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using FlotixPlatform.Api.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using Microsoft.EntityFrameworkCore;

namespace FlotixPlatform.Api.Notifications.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

/// <summary>
///     Entity Framework repository for alert persistence.
/// </summary>
/// <remarks>
///     This repository extends the generic BaseRepository with alert-specific query methods.
/// </remarks>
/// <param name="context">The EF Core database context.</param>
public class AlertRepository(AppDbContext context)
    : BaseRepository<Alert>(context), IAlertRepository
{
    /// <inheritdoc />
    public async Task<IEnumerable<Alert>> FindByVehicleIdAsync(Guid vehicleId, CancellationToken cancellationToken = default)
    {
        return await Context.Set<Alert>()
            .Where(a => a.VehicleId == vehicleId)
            .ToListAsync(cancellationToken);
    }
}
