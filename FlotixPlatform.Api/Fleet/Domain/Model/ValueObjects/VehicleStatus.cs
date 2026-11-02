using System.Text.Json.Serialization;

namespace FlotixPlatform.Api.Fleet.Domain.Model.ValueObjects;

/// <summary>
///     Represents the operational status of a vehicle.
/// </summary>
/// <remarks>
///     Serialized with the Spanish names used by the Flotix front end and persisted by member name.
/// </remarks>
public enum VehicleStatus
{
    /// <summary>
    ///     The vehicle is available for operation.
    /// </summary>
    [JsonStringEnumMemberName("Disponible")]
    Available,

    /// <summary>
    ///     The vehicle is under maintenance.
    /// </summary>
    [JsonStringEnumMemberName("En mantenimiento")]
    InMaintenance,

    /// <summary>
    ///     The vehicle is out of service.
    /// </summary>
    [JsonStringEnumMemberName("Inactivo")]
    Inactive
}