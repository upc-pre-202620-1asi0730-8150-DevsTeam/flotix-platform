namespace FlotixPlatform.Api.Fuel.Domain.Model.Queries;

/// <summary>
///     Query to get all fuel records by driver ID
/// </summary>
/// <param name="DriverId">The driver ID to search</param>
public record GetAllFuelRecordsByDriverIdQuery(Guid DriverId);
