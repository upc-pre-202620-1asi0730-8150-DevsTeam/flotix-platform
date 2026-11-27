using FlotixPlatform.Api.Monitoring.Domain.Model.Commands;
using FlotixPlatform.Api.Monitoring.Domain.Model.ValueObjects;
using FlotixPlatform.Api.Monitoring.Interfaces.Rest.Resources;

namespace FlotixPlatform.Api.Monitoring.Interfaces.Rest.Transform;

public static class CreateIotDeviceCommandFromResourceAssembler
{
    public static CreateIotDeviceCommand ToCommandFromResource(CreateIotDeviceResource resource) =>
        new(resource.VehicleId, new SerialNumber(resource.SerialNumber), resource.IsConnected);
}
