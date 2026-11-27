using FlotixPlatform.Api.Monitoring.Application.Internal.OutboundServices;
using FlotixPlatform.Api.Monitoring.Domain.Model.Aggregates;
using FlotixPlatform.Api.Monitoring.Interfaces.Hubs;
using FlotixPlatform.Api.Monitoring.Interfaces.Rest.Transform;
using Microsoft.AspNetCore.SignalR;

namespace FlotixPlatform.Api.Monitoring.Infrastructure.Realtime.SignalR.Services;

public class VehicleLocationNotifier(IHubContext<MonitoringHub, IMonitoringClient> monitoringHub) : IVehicleLocationNotifier
{
    public Task NotifyAsync(Guid vehicleId, TelemetryData telemetryData, CancellationToken cancellationToken = default) =>
        monitoringHub.Clients.All.VehicleLocationUpdated(
            VehicleLocationResourceFromEntityAssembler.ToResourceFromEntity(vehicleId, telemetryData));
}
