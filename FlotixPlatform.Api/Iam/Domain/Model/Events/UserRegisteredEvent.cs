using FlotixPlatform.Api.Iam.Domain.Model.ValueObjects;
using FlotixPlatform.Api.Shared.Domain.Model.Events;

namespace FlotixPlatform.Api.Iam.Domain.Model.Events;

/// <summary>
///     Domain event raised when a user is registered in the IAM context.
/// </summary>
/// <param name="UserId">The strongly-typed identifier of the registered user.</param>
/// <param name="Username">The username of the registered user.</param>
public record UserRegisteredEvent(UserId UserId, string Username) : IEvent;
