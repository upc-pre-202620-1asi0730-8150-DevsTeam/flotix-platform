using System.ComponentModel.DataAnnotations;

namespace FlotixPlatform.API.Maintenance.Interfaces.Rest.Resources;

/// <summary>
///     Represents the data required to submit a maintenance request to a mechanic.
/// </summary>
/// <param name="VehicleId">The identifier of the vehicle.</param>
/// <param name="MechanicId">The identifier of the mechanic user that attends the request.</param>
/// <param name="Description">The description of the fault to repair. (required)</param>
public record SubmitMaintenanceRequestResource(
    Guid VehicleId,
    Guid MechanicId,
    [Required(ErrorMessage = "RequiredDescription")]
    [StringLength(500, ErrorMessage = "StringLengthField")]
    string Description);
