using System.ComponentModel.DataAnnotations;

namespace FlotixPlatform.Api.Notifications.Interfaces.Rest.Resources;

/// <summary>
///     Represents the data required to create an alert.
/// </summary>
/// <param name="VehicleId">The identifier of the vehicle.</param>
/// <param name="AlertType">The type of the alert (e.g. Exceso de velocidad). (required)</param>
public record CreateAlertResource(
    Guid VehicleId,
    [Required(ErrorMessage = "RequiredAlertType")]
    [StringLength(50, ErrorMessage = "StringLengthField")]
    string AlertType);
