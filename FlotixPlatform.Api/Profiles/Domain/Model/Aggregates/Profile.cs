using FlotixPlatform.Api.Profiles.Domain.Model.Commands;
using FlotixPlatform.Api.Profiles.Domain.Model.Events;
using FlotixPlatform.Api.Profiles.Domain.Model.ValueObjects;
using FlotixPlatform.Api.Shared.Domain.Model.Aggregates.Internal;

namespace FlotixPlatform.Api.Profiles.Domain.Model.Aggregates;

/// <summary>
///     Profile Aggregate Root
/// </summary>
/// <remarks>
///     This class represents the Profile aggregate root.
///     It contains the properties and methods to manage the profile information of an
///     owner, a driver or a mechanic of the Flotix Platform.
/// </remarks>
public partial class Profile : AggregateRoot
{
    public Profile()
    {
        Name = new PersonName();
        Email = new EmailAddress();
        Role = EProfileRole.Owner;
    }

    public Profile(string firstName, string lastName, string email, EProfileRole role)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(firstName);
        ArgumentException.ThrowIfNullOrWhiteSpace(lastName);
        ArgumentException.ThrowIfNullOrWhiteSpace(email);
        Name = new PersonName(firstName, lastName);
        Email = new EmailAddress(email);
        Role = role;
        AddDomainEvent(new ProfileCreatedEvent(Email, Role));
    }

    public Profile(CreateProfileCommand command) : this(command.FirstName, command.LastName, command.Email,
        command.Role)
    {
    }

    public ProfileId Id { get; }
    public PersonName Name { get; private set; }
    public EmailAddress Email { get; private set; }
    public EProfileRole Role { get; private set; }

    public string FullName => Name.FullName;
    public string EmailAddress => Email.Address;
}
