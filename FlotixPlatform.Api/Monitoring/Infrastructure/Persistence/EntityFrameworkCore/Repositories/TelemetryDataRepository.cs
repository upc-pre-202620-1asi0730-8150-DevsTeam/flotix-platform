using FlotixPlatform.Api.Monitoring.Domain.Model.Aggregates;
using FlotixPlatform.Api.Monitoring.Domain.Repositories;
using FlotixPlatform.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using FlotixPlatform.Api.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using Microsoft.EntityFrameworkCore;

namespace FlotixPlatform.API.Monitoring.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

public class TelemetryDataRepository(AppDbContext context)
    : BaseRepository<TelemetryData>(context), ITelemetryDataRepository
{
    public async Task<IEnumerable<TelemetryData>> FindByDeviceIdAsync(Guid deviceId, CancellationToken cancellationToken = default)
    {
        return await Context.Set<TelemetryData>()
            .Where(t => t.DeviceId == deviceId)
            .OrderByDescending(t => t.RecordedAt)
            .ToListAsync(cancellationToken);
    }
}