namespace FlotixPlatform.Api.Notifications.Interfaces.Rest.Resources;

/// <summary>
///     Represents the data provided by the server about an alert.
/// </summary>
/// <param name="Id">The server-generated ID of the alert</param>
/// <param name="VehicleId">The identifier of the vehicle.</param>
/// <param name="AlertType">The type of the alert (e.g. Exceso de velocidad).</param>
/// <param name="TriggerDate">The moment the alert was triggered.</param>
public record AlertResource(
    Guid Id,
    Guid VehicleId,
    string AlertType,
    DateTimeOffset TriggerDate);
