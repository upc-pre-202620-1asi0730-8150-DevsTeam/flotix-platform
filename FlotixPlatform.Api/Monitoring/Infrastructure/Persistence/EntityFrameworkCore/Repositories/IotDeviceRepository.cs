using FlotixPlatform.Api.Monitoring.Domain.Model.Aggregates;
using FlotixPlatform.Api.Monitoring.Domain.Model.ValueObjects;
using FlotixPlatform.Api.Monitoring.Domain.Repositories;
using FlotixPlatform.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using FlotixPlatform.Api.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using Microsoft.EntityFrameworkCore;

namespace FlotixPlatform.API.Monitoring.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

public class IotDeviceRepository(AppDbContext context)
    : BaseRepository<IotDevice>(context), IIotDeviceRepository
{
    public async Task<IEnumerable<IotDevice>> FindByVehicleIdAsync(Guid vehicleId, CancellationToken cancellationToken = default)
    {
        return await Context.Set<IotDevice>()
            .Where(i => i.VehicleId == vehicleId)
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> ExistsByVehicleIdAsync(Guid vehicleId, CancellationToken cancellationToken = default)
    {
        return await Context.Set<IotDevice>().AnyAsync(i => i.VehicleId == vehicleId, cancellationToken);
    }

    public async Task<bool> ExistsBySerialNumberAsync(SerialNumber serialNumber, CancellationToken cancellationToken = default)
    {
        return await Context.Set<IotDevice>().AnyAsync(i => i.SerialNumber == serialNumber, cancellationToken);
    }

    public async Task<IotDevice?> FindBySerialNumberAsync(SerialNumber serialNumber, CancellationToken cancellationToken = default)
    {
        return await Context.Set<IotDevice>().FirstOrDefaultAsync(i => i.SerialNumber == serialNumber, cancellationToken);
    }
}