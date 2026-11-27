using FlotixPlatform.Api.Profiles.Domain.Model.ValueObjects;
using FlotixPlatform.Api.Shared.Domain.Model.Events;

namespace FlotixPlatform.Api.Profiles.Domain.Model.Events;

/// <summary>
///     Domain event raised when a new profile is created.
/// </summary>
/// <param name="Email">The email address of the created profile.</param>
/// <param name="Role">The role of the created profile.</param>
public record ProfileCreatedEvent(EmailAddress Email, EProfileRole Role) : IEvent;
