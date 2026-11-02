namespace FlotixPlatform.Api.Fuel.Domain.Model.Queries;

/// <summary>
///     Query to get a fuel record by ID
/// </summary>
/// <param name="Id">The fuel record ID to search</param>
public record GetFuelRecordByIdQuery(Guid Id);
