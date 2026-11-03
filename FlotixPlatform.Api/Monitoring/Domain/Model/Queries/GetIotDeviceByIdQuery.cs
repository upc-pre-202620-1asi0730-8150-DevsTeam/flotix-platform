namespace FlotixPlatform.Api.Monitoring.Domain.Model.Queries;

/// <summary>
///     Query to get an IoT device by ID
/// </summary>
/// <param name="Id">The IoT device ID to search</param>
public record GetIotDeviceByIdQuery(Guid Id);
