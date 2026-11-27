using FlotixPlatform.Api.Profiles.Domain.Model.Events;
using FlotixPlatform.Api.Shared.Application.Internal.EventHandlers;
using Microsoft.Extensions.Logging;

namespace FlotixPlatform.Api.Profiles.Application.Internal.EventHandlers;

/// <summary>
///     Event handler for <see cref="ProfileCreatedEvent" />.
/// </summary>
public class ProfileCreatedEventHandler(ILogger<ProfileCreatedEventHandler> logger) : IEventHandler<ProfileCreatedEvent>
{
    /// <inheritdoc />
    public Task Handle(ProfileCreatedEvent domainEvent, CancellationToken cancellationToken)
    {
        logger.LogInformation("Profile created: {Email} ({Role})", domainEvent.Email.Address, domainEvent.Role);
        return Task.CompletedTask;
    }
}
