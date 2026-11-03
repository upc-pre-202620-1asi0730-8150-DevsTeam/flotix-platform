namespace FlotixPlatform.API.Commerce.Domain.Model.Queries;

/// <summary>
///     Query to get an IoT order by ID
/// </summary>
/// <param name="Id">The IoT order ID to search</param>
public record GetIotOrderByIdQuery(Guid Id);
