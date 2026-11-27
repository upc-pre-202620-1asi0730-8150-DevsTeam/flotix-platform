using FlotixPlatform.Api.Monitoring.Domain.Model.Commands;
using FlotixPlatform.Api.Monitoring.Interfaces.Rest.Resources;

namespace FlotixPlatform.Api.Monitoring.Interfaces.Rest.Transform;

public static class UpdateIotDeviceCommandFromResourceAssembler
{
    public static UpdateIotDeviceCommand ToCommandFromResource(Guid iotDeviceId, UpdateIotDeviceResource resource) =>
        new(iotDeviceId, resource.VehicleId, resource.IsConnected);
}
