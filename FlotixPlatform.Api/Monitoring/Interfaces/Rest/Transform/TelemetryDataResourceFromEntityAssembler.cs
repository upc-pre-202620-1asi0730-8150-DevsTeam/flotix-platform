using FlotixPlatform.Api.Monitoring.Domain.Model.Aggregates;
using FlotixPlatform.Api.Monitoring.Interfaces.Rest.Resources;

namespace FlotixPlatform.Api.Monitoring.Interfaces.Rest.Transform;

public static class TelemetryDataResourceFromEntityAssembler
{
    public static TelemetryDataResource ToResourceFromEntity(TelemetryData entity) =>
        new(entity.Id, entity.DeviceId, entity.Latitude, entity.Longitude, entity.Speed, entity.RecordedAt);
}
