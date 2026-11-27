using FlotixPlatform.Api.Incidents.Domain.Model.Aggregates;
using FlotixPlatform.Api.Incidents.Interfaces.Rest.Resources;

namespace FlotixPlatform.Api.Incidents.Interfaces.Rest.Transform;

/// <summary>
///     Assembles a IncidentResource from a Incident.
/// </summary>
public static class IncidentResourceFromEntityAssembler
{
    /// <summary>
    ///     Assembles a IncidentResource from a Incident.
    /// </summary>
    /// <param name="entity">The Incident entity</param>
    /// <returns>
    ///     A IncidentResource assembled from the Incident
    /// </returns>
    public static IncidentResource ToResourceFromEntity(Incident entity) =>
        new(entity.Id, entity.VehicleId, entity.DriverId, entity.Description, entity.Status, entity.ReportedDate);
}
