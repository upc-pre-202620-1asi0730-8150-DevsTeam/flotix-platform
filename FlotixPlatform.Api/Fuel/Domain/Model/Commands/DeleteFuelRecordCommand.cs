namespace FlotixPlatform.Api.Fuel.Domain.Model.Commands;

/// <summary>
///     Command to delete a fuel record
/// </summary>
/// <param name="FuelRecordId">The identifier of the fuel record to delete</param>
public record DeleteFuelRecordCommand(Guid FuelRecordId);
