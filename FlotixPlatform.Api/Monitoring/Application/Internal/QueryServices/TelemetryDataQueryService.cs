using FlotixPlatform.Api.Monitoring.Domain.Model.Aggregates;
using FlotixPlatform.Api.Monitoring.Application.Services;
using FlotixPlatform.Api.Monitoring.Domain.Model.Queries;
using FlotixPlatform.Api.Monitoring.Domain.Repositories;

namespace FlotixPlatform.Api.Monitoring.Application.Internal.QueryServices;

/// <summary>
///     Application service for querying telemetry records.
/// </summary>
/// <remarks>
///     This service acts as the query handler for retrieving telemetry records.
///     All queries delegate to the repository layer for data access.
/// </remarks>
/// <param name="telemetryDataRepository">Repository for accessing telemetry record data.</param>
public class TelemetryDataQueryService(ITelemetryDataRepository telemetryDataRepository)
    : ITelemetryDataQueryService
{
    /// <inheritdoc />
    public async Task<IEnumerable<TelemetryData>> Handle(GetAllTelemetryDataQuery query,
        CancellationToken cancellationToken = default) =>
        await telemetryDataRepository.ListAsync(cancellationToken);

    /// <inheritdoc />
    public async Task<TelemetryData?> Handle(GetTelemetryDataByIdQuery query,
        CancellationToken cancellationToken = default) =>
        await telemetryDataRepository.FindByIdAsync(query.Id, cancellationToken);

    /// <inheritdoc />
    public async Task<IEnumerable<TelemetryData>> Handle(GetAllTelemetryDataByDeviceIdQuery query,
        CancellationToken cancellationToken = default) =>
        await telemetryDataRepository.FindByDeviceIdAsync(query.DeviceId, cancellationToken);
}
