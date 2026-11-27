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
        if (command.Plan is { } plan) ActivatePlan(plan, command.PaymentCardLast4, command.PaymentCardExpiry);
    }

    public ProfileId Id { get; }
    public PersonName Name { get; private set; }
    public EmailAddress Email { get; private set; }
    public EProfileRole Role { get; private set; }

    /// <summary>Subscription plan bought by the owner, or <c>null</c> when the profile has no plan.</summary>
    public EProfilePlan? Plan { get; private set; }

    public decimal? PlanPriceMonthly { get; private set; }
    public DateTimeOffset? PlanStartedAt { get; private set; }
    public DateTimeOffset? PlanRenewsAt { get; private set; }

    /// <summary>Last 4 digits of the payment card (simulated purchase). The full number is never stored.</summary>
    public string? PaymentCardLast4 { get; private set; }

    public string? PaymentCardExpiry { get; private set; }

    public string FullName => Name.FullName;
    public string EmailAddress => Email.Address;

    /// <summary>
    ///     Activates a subscription plan for this profile. Only owners can subscribe to a plan.
    /// </summary>
    public void ActivatePlan(EProfilePlan plan, string? cardLast4, string? cardExpiry)
    {
        if (Role != EProfileRole.Owner)
            throw new InvalidOperationException("Only owners can subscribe to a plan.");

        var now = DateTimeOffset.UtcNow;
        Plan = plan;
        PlanPriceMonthly = plan.MonthlyPrice();
        PlanStartedAt = now;
        PlanRenewsAt = now.AddMonths(1);
        PaymentCardLast4 = cardLast4;
        PaymentCardExpiry = cardExpiry;
    }
}
