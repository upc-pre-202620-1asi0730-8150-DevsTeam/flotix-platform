using FlotixPlatform.Api.Monitoring.Interfaces.Rest.Resources;

namespace FlotixPlatform.Api.Monitoring.Interfaces.Hubs;

public interface IMonitoringClient
{
    Task VehicleLocationUpdated(VehicleLocationResource vehicleLocation);
}
