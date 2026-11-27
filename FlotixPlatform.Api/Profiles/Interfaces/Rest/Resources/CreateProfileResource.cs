using System.ComponentModel.DataAnnotations;

namespace FlotixPlatform.Api.Profiles.Interfaces.Rest.Resources;

/// <summary>
///     Resource for creating a new profile
/// </summary>
/// <param name="FirstName">
///     The first name of the profile
/// </param>
/// <param name="LastName">
///     The last name of the profile
/// </param>
/// <param name="Email">
///     The email of the profile
/// </param>
/// <param name="Role">
///     The role of the profile: Owner, Driver or Mechanic
/// </param>
/// <param name="Plan">
///     Optional subscription plan bought at sign-up: Starter, Growth or Fleet (owners only)
/// </param>
/// <param name="PaymentCardLast4">
///     Optional last 4 digits of the payment card used for the (simulated) purchase
/// </param>
/// <param name="PaymentCardExpiry">
///     Optional expiry date of the payment card, MM/YY
/// </param>
public record CreateProfileResource(
    [Required]
    [MaxLength(50, ErrorMessage = "The first name must not exceed 50 characters.")]
    string FirstName,
    [Required]
    [MaxLength(50, ErrorMessage = "The last name must not exceed 50 characters.")]
    string LastName,
    [Required]
    [EmailAddress]
    [MaxLength(100, ErrorMessage = "The email must not exceed 100 characters.")]
    string Email,
    [Required]
    [AllowedValues("Owner", "Driver", "Mechanic", ErrorMessage = "The role must be Owner, Driver or Mechanic.")]
    string Role,
    [RegularExpression("(?i)^(starter|growth|fleet)$", ErrorMessage = "The plan must be Starter, Growth or Fleet.")]
    string? Plan = null,
    [RegularExpression(@"^\d{4}$", ErrorMessage = "The card last 4 digits must be exactly 4 digits.")]
    string? PaymentCardLast4 = null,
    [RegularExpression(@"^(0[1-9]|1[0-2])/\d{2}$", ErrorMessage = "The card expiry must use the MM/YY format.")]
    string? PaymentCardExpiry = null);
