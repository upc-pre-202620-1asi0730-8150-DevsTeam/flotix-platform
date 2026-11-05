using System.Text.Json.Serialization;

namespace FlotixPlatform.Api.Incidents.Domain.Model.ValueObjects;

/// <summary>
///     Represents the resolution status of an incident.
/// </summary>
/// <remarks>
///     Serialized with the Spanish names used by the Flotix front end and persisted by member name.
/// </remarks>
public enum IncidentStatus
{
    /// <summary>
    ///     The incident has just been reported.
    /// </summary>
    [JsonStringEnumMemberName("Abierto")]
    Open,

    /// <summary>
    ///     The incident is being reviewed.
    /// </summary>
    [JsonStringEnumMemberName("En revisión")]
    InReview,

    /// <summary>
    ///     The incident has been resolved.
    /// </summary>
    [JsonStringEnumMemberName("Resuelto")]
    Resolved
}
