using FlotixPlatform.Api.Monitoring.Domain.Model.Aggregates;
using FlotixPlatform.Api.Monitoring.Interfaces.Rest.Resources;

namespace FlotixPlatform.Api.Monitoring.Interfaces.Rest.Transform;

public static class VehicleLocationResourceFromEntityAssembler
{
    public static VehicleLocationResource ToResourceFromEntity(Guid vehicleId, TelemetryData entity) =>
        new(vehicleId, entity.DeviceId, entity.Latitude, entity.Longitude, entity.Speed, entity.RecordedAt);
}
