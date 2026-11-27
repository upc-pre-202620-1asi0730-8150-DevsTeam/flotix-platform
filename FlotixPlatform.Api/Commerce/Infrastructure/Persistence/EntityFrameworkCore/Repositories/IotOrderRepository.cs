using FlotixPlatform.Api.Commerce.Domain.Model.Aggregates;
using FlotixPlatform.Api.Commerce.Domain.Repositories;
using FlotixPlatform.Api.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using FlotixPlatform.Api.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using Microsoft.EntityFrameworkCore;

namespace FlotixPlatform.Api.Commerce.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

/// <summary>
///     Entity Framework repository for IoT order persistence.
/// </summary>
/// <remarks>
///     This repository extends the generic BaseRepository with IoT order-specific query methods.
/// </remarks>
/// <param name="context">The EF Core database context.</param>
public class IotOrderRepository(AppDbContext context)
    : BaseRepository<IotOrder>(context), IIotOrderRepository
{
    /// <inheritdoc />
    public async Task<IEnumerable<IotOrder>> FindByOwnerIdAsync(Guid ownerId, CancellationToken cancellationToken = default)
    {
        return await Context.Set<IotOrder>()
            .Where(i => i.OwnerId == ownerId)
            .ToListAsync(cancellationToken);
    }
}
