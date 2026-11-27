using FlotixPlatform.Api.Monitoring.Domain.Model.Commands;
using FlotixPlatform.Api.Monitoring.Interfaces.Rest.Resources;

namespace FlotixPlatform.Api.Monitoring.Interfaces.Rest.Transform;

public static class ReceiveTelemetryDataCommandFromResourceAssembler
{
    public static ReceiveTelemetryDataCommand ToCommandFromResource(ReceiveTelemetryDataResource resource) =>
        new(Required(resource.DeviceId, nameof(resource.DeviceId)),
            Required(resource.Latitude, nameof(resource.Latitude)),
            Required(resource.Longitude, nameof(resource.Longitude)),
            Required(resource.Speed, nameof(resource.Speed)),
            Required(resource.RecordedAt, nameof(resource.RecordedAt)));

    private static T Required<T>(T? value, string name) where T : struct =>
        value ?? throw new ArgumentException($"The telemetry packet has no {name}.", name);
}
