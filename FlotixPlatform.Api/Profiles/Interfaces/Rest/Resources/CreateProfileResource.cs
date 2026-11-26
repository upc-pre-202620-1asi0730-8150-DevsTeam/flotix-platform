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
    string Role);
