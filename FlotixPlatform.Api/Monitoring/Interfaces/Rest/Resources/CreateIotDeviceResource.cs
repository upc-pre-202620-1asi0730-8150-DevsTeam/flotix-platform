using System.ComponentModel.DataAnnotations;

namespace FlotixPlatform.Api.Monitoring.Interfaces.Rest.Resources;

public record CreateIotDeviceResource(
    Guid VehicleId,
    [Required(ErrorMessage = "RequiredSerialNumber")]
    [StringLength(100, ErrorMessage = "StringLengthField")]
    string SerialNumber,
    bool IsConnected);
