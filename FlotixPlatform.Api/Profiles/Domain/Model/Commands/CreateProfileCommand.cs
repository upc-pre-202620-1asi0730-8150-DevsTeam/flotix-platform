using FlotixPlatform.Api.Profiles.Domain.Model.ValueObjects;

namespace FlotixPlatform.Api.Profiles.Domain.Model.Commands;

/// <summary>
///     Create Profile Command
/// </summary>
/// <param name="FirstName">
///     The first name of the profile.
/// </param>
/// <param name="LastName">
///     The last name of the profile.
/// </param>
/// <param name="Email">
///     The email address of the profile.
/// </param>
/// <param name="Role">
///     The <see cref="EProfileRole" /> role of the profile.
/// </param>
/// <param name="Plan">
///     The optional <see cref="EProfilePlan" /> bought at sign-up. Only owners can have a plan.
/// </param>
/// <param name="PaymentCardLast4">
///     Last 4 digits of the (simulated) payment card. The full card number is never stored.
/// </param>
/// <param name="PaymentCardExpiry">
///     Expiry date of the (simulated) payment card in MM/YY format.
/// </param>
public record CreateProfileCommand(
    string FirstName,
    string LastName,
    string Email,
    EProfileRole Role,
    EProfilePlan? Plan = null,
    string? PaymentCardLast4 = null,
    string? PaymentCardExpiry = null);
