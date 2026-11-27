namespace FlotixPlatform.Api.Iam.Interfaces.Rest.Resources;

/// <summary>
///     Authenticated user resource for REST API
/// </summary>
/// <param name="Id">The IAM user id</param>
/// <param name="Username">The username</param>
/// <param name="Token">The bearer token</param>
/// <param name="OwnerId">The owner UUID used by the fleet resources (vehicles, IoT orders, reports, notifications)</param>
public record AuthenticatedUserResource(int Id, string Username, string Token, Guid OwnerId);