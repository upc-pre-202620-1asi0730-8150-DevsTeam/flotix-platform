using FlotixPlatform.Api.Incidents.Domain.Model.Aggregates;
using FlotixPlatform.Api.Incidents.Application.Services;
using FlotixPlatform.Api.Incidents.Domain.Model.Queries;
using FlotixPlatform.Api.Incidents.Domain.Repositories;

namespace FlotixPlatform.Api.Incidents.Application.Internal.QueryServices;

/// <summary>
///     Application service for querying incidents.
/// </summary>
/// <remarks>
///     This service acts as the query handler for retrieving incidents.
///     All queries delegate to the repository layer for data access.
/// </remarks>
/// <param name="incidentRepository">Repository for accessing incident data.</param>
public class IncidentQueryService(IIncidentRepository incidentRepository)
    : IIncidentQueryService
{
    /// <inheritdoc />
    public async Task<IEnumerable<Incident>> Handle(GetAllIncidentsQuery query,
        CancellationToken cancellationToken = default) =>
        await incidentRepository.ListAsync(cancellationToken);

    /// <inheritdoc />
    public async Task<Incident?> Handle(GetIncidentByIdQuery query,
        CancellationToken cancellationToken = default) =>
        await incidentRepository.FindByIdAsync(query.Id, cancellationToken);

    /// <inheritdoc />
    public async Task<IEnumerable<Incident>> Handle(GetAllIncidentsByVehicleIdQuery query,
        CancellationToken cancellationToken = default) =>
        await incidentRepository.FindByVehicleIdAsync(query.VehicleId, cancellationToken);

    /// <inheritdoc />
    public async Task<IEnumerable<Incident>> Handle(GetAllIncidentsByDriverIdQuery query,
        CancellationToken cancellationToken = default) =>
        await incidentRepository.FindByDriverIdAsync(query.DriverId, cancellationToken);
}
