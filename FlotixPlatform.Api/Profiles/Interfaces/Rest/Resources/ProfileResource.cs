namespace FlotixPlatform.Api.Profiles.Interfaces.Rest.Resources;

/// <summary>
///     Profile resource for REST API
/// </summary>
/// <param name="Id">
///     The unique identifier of the profile
/// </param>
/// <param name="FullName">
///     The full name of the profile
/// </param>
/// <param name="Email">
///     The email address of the profile
/// </param>
/// <param name="Role">
///     The role of the profile
/// </param>
/// <param name="Plan">
///     The subscription plan of the profile (Starter, Growth or Fleet), or null when it has none
/// </param>
/// <param name="PlanPriceMonthly">
///     The monthly price of the plan in soles (S/)
/// </param>
/// <param name="PlanStartedAt">
///     When the plan was activated
/// </param>
/// <param name="PlanRenewsAt">
///     When the plan renews next
/// </param>
/// <param name="PaymentCardLast4">
///     Last 4 digits of the payment card
/// </param>
/// <param name="PaymentCardExpiry">
///     Expiry date of the payment card, MM/YY
/// </param>
public record ProfileResource(
    int Id,
    string FullName,
    string Email,
    string Role,
    string? Plan = null,
    decimal? PlanPriceMonthly = null,
    DateTimeOffset? PlanStartedAt = null,
    DateTimeOffset? PlanRenewsAt = null,
    string? PaymentCardLast4 = null,
    string? PaymentCardExpiry = null);
