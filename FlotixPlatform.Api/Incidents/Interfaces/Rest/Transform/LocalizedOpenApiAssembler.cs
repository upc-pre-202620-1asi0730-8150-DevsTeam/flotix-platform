using FlotixPlatform.Api.Incidents.Interfaces.Rest.Resources;
using FlotixPlatform.Api.Resources;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.Extensions.Localization;
using Microsoft.OpenApi;

namespace FlotixPlatform.Api.Incidents.Interfaces.Rest.Transform;

/// <summary>
///     Assembles localized OpenAPI metadata (summaries, descriptions, and response texts)
///     for the Incident Management REST endpoints and resources.
/// </summary>
public static class LocalizedOpenApiAssembler
{
    private static readonly Dictionary<string, (string Summary, string Description,
        IReadOnlyDictionary<string, string> Responses)> OperationResources =
        new Dictionary<string, (string, string, IReadOnlyDictionary<string, string>)>
        {
            ["CreateIncident"] = (
                "CreateIncidentSummary",
                "CreateIncidentDescription",
                new Dictionary<string, string>
                {
                    ["201"] = "Response201",
                    ["400"] = "Response400",
                    ["404"] = "Response404",
                    ["500"] = "Response500"
                }),
            ["GetAllIncidents"] = (
                "GetAllIncidentsSummary",
                "GetAllIncidentsDescription",
                new Dictionary<string, string>
                {
                    ["200"] = "Response200"
                }),
            ["GetIncidentById"] = (
                "GetIncidentByIdSummary",
                "GetIncidentByIdDescription",
                new Dictionary<string, string>
                {
                    ["200"] = "Response200",
                    ["404"] = "Response404"
                }),
            ["UpdateIncident"] = (
                "UpdateIncidentSummary",
                "UpdateIncidentDescription",
                new Dictionary<string, string>
                {
                    ["200"] = "Response200",
                    ["400"] = "Response400",
                    ["404"] = "Response404",
                    ["500"] = "Response500"
                }),
            ["DeleteIncident"] = (
                "DeleteIncidentSummary",
                "DeleteIncidentDescription",
                new Dictionary<string, string>
                {
                    ["204"] = "Response204",
                    ["404"] = "Response404",
                    ["500"] = "Response500"
                })
        };

    private static readonly Dictionary<Type, (string Schema, string Owner)> SchemaResources = new()
    {
            [typeof(CreateIncidentResource)] = ("CreateIncidentSchema", "Incident"),
            [typeof(UpdateIncidentResource)] = ("UpdateIncidentSchema", "Incident"),
            [typeof(IncidentResource)] = ("IncidentSchema", "Incident")
    };

    /// <summary>
    ///     Localizes the summary, description, operation id and response descriptions of known operations.
    /// </summary>
    /// <param name="operation">The OpenAPI operation being transformed.</param>
    /// <param name="context">The transformer context, used to resolve the localizer and action descriptor.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A completed task.</returns>
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

    /// <summary>
    ///     Localizes the description of known resource schemas and their properties.
    /// </summary>
    /// <param name="schema">The OpenAPI schema being transformed.</param>
    /// <param name="context">The transformer context, used to resolve the localizer and type information.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A completed task.</returns>
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
