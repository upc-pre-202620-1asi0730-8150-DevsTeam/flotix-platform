namespace FlotixPlatform.Api.Monitoring.Domain.Model.Queries;

/// <summary>
///     Query to get a telemetry record by ID
/// </summary>
/// <param name="Id">The telemetry record ID to search</param>
public record GetTelemetryDataByIdQuery(Guid Id);
