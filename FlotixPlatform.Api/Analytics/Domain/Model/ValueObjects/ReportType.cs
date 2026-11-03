using System.Text.Json.Serialization;

namespace FlotixPlatform.Api.Analytics.Domain.Model.ValueObjects;

/// <summary>
///     Represents the types of report the platform generates.
/// </summary>
/// <remarks>
///     Serialized with the Spanish names used by the Flotix front end and persisted by member name.
/// </remarks>
public enum ReportType
{
    /// <summary>
    ///     Fuel consumption report: refuels, liters and fuel cost per vehicle.
    /// </summary>
    [JsonStringEnumMemberName("Reporte de consumo de combustible")]
    FuelConsumption,

    /// <summary>
    ///     Maintenance cost report: maintenance requests, completed repairs and approved budgets per vehicle.
    /// </summary>
    [JsonStringEnumMemberName("Reporte de costos de mantenimiento")]
    MaintenanceCost
}
