using FlotixPlatform.Api.Monitoring.Domain.Model.Aggregates;
using FlotixPlatform.Api.Monitoring.Interfaces.Rest.Resources;

namespace FlotixPlatform.Api.Monitoring.Interfaces.Rest.Transform;

public static class IotDeviceResourceFromEntityAssembler
{
    public static IotDeviceResource ToResourceFromEntity(IotDevice entity) =>
        new(entity.Id, entity.VehicleId, entity.SerialNumber.Value, entity.IsConnected);
}
