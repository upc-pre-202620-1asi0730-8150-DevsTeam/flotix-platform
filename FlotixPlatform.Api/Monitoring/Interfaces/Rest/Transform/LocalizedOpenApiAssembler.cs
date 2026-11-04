using FlotixPlatform.Api.Monitoring.Interfaces.Rest.Resources;
using FlotixPlatform.Api.Resources;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.Extensions.Localization;
using Microsoft.OpenApi;

namespace FlotixPlatform.API.Monitoring.Interfaces.Rest.Transform;

public static class LocalizedOpenApiAssembler
{
    private static readonly Dictionary<string, (string Summary, string Description,
        IReadOnlyDictionary<string, string> Responses)> OperationResources =
        new Dictionary<string, (string, string, IReadOnlyDictionary<string, string>)>
        {
            ["CreateIotDevice"] = (
                "CreateIotDeviceSummary",
                "CreateIotDeviceDescription",
                new Dictionary<string, string>
                {
                    ["201"] = "Response201",
                    ["400"] = "Response400",
                    ["404"] = "Response404",
                    ["409"] = "Response409",
                    ["500"] = "Response500"
                }),
            ["GetAllIotDevices"] = (
                "GetAllIotDevicesSummary",
                "GetAllIotDevicesDescription",
                new Dictionary<string, string>
                {
                    ["200"] = "Response200"
                }),
            ["GetIotDeviceById"] = (
                "GetIotDeviceByIdSummary",
                "GetIotDeviceByIdDescription",
                new Dictionary<string, string>
                {
                    ["200"] = "Response200",
                    ["404"] = "Response404"
                }),
            ["UpdateIotDevice"] = (
                "UpdateIotDeviceSummary",
                "UpdateIotDeviceDescription",
                new Dictionary<string, string>
                {
                    ["200"] = "Response200",
                    ["400"] = "Response400",
                    ["404"] = "Response404",
                    ["409"] = "Response409",
                    ["500"] = "Response500"
                }),
            ["DeleteIotDevice"] = (
                "DeleteIotDeviceSummary",
                "DeleteIotDeviceDescription",
                new Dictionary<string, string>
                {
                    ["204"] = "Response204",
                    ["404"] = "Response404",
                    ["409"] = "Response409",
                    ["500"] = "Response500"
                }),
            ["ReceiveTelemetryData"] = (
                "ReceiveTelemetryDataSummary",
                "ReceiveTelemetryDataDescription",
                new Dictionary<string, string>
                {
                    ["202"] = "Response202",
                    ["400"] = "Response400",
                    ["500"] = "Response500"
                }),
            ["GetAllTelemetryData"] = (
                "GetAllTelemetryDataSummary",
                "GetAllTelemetryDataDescription",
                new Dictionary<string, string>
                {
                    ["200"] = "Response200"
                }),
            ["GetTelemetryDataById"] = (
                "GetTelemetryDataByIdSummary",
                "GetTelemetryDataByIdDescription",
                new Dictionary<string, string>
                {
                    ["200"] = "Response200",
                    ["404"] = "Response404"
                })
        };

    private static readonly Dictionary<Type, (string Schema, string Owner)> SchemaResources = new()
    {
            [typeof(CreateIotDeviceResource)] = ("CreateIotDeviceSchema", "IotDevice"),
            [typeof(UpdateIotDeviceResource)] = ("UpdateIotDeviceSchema", "IotDevice"),
            [typeof(IotDeviceResource)] = ("IotDeviceSchema", "IotDevice"),
            [typeof(ReceiveTelemetryDataResource)] = ("ReceiveTelemetryDataSchema", "TelemetryData"),
            [typeof(VehicleLocationResource)] = ("VehicleLocationSchema", "VehicleLocation"),
            [typeof(TelemetryDataResource)] = ("TelemetryDataSchema", "TelemetryData")
    };

    public static Task TransformOperationAsync(
        OpenApiOperation operation,
        OpenApiOperationTransformerContext context,
        CancellationToken cancellationToken)
    {
        if (context.Description.ActionDescriptor is not ControllerActionDescriptor action ||
            !OperationResources.TryGetValue(action.ActionName, out var resources))
            return Task.CompletedTask;

        var localizer = context.ApplicationServices.GetRequiredService<IStringLocalizer<SharedResource>>();
        operation.OperationId = action.ActionName;
        operation.Summary = localizer[resources.Summary].Value;
        operation.Description = localizer[resources.Description].Value;
        if (operation.Responses is null) return Task.CompletedTask;

        foreach (var (statusCode, resourceKey) in resources.Responses)
        {
            if (operation.Responses.TryGetValue(statusCode, out var response) &&
                response is OpenApiResponse openApiResponse)
                openApiResponse.Description = localizer[resourceKey].Value;
        }

        return Task.CompletedTask;
    }

    public static Task TransformSchemaAsync(
        OpenApiSchema schema,
        OpenApiSchemaTransformerContext context,
        CancellationToken cancellationToken)
    {
        var localizer = context.ApplicationServices.GetRequiredService<IStringLocalizer<SharedResource>>();
        if (SchemaResources.TryGetValue(context.JsonTypeInfo.Type, out var schemaResources))
            schema.Description = localizer[schemaResources.Schema].Value;

        if (context.JsonPropertyInfo is { } property &&
            SchemaResources.TryGetValue(property.DeclaringType, out var ownerResources))
        {
            var propertyResourceKey = ownerResources.Owner + char.ToUpperInvariant(property.Name[0]) + property.Name[1..];
            schema.Description = localizer[propertyResourceKey].Value;
        }

        return Task.CompletedTask;
    }
}
