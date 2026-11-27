namespace FlotixPlatform.Api.DemoData.Infrastructure.Configuration;

/// <summary>
///     Demo data settings, bound from the <c>DemoData</c> configuration section.
/// </summary>
public class DemoDataSettings
{
    /// <summary>
    ///     Generates the demo fleet of every new account right after sign-up. Default: <c>true</c>.
    ///     Override with the <c>DemoData__SeedOnSignUp</c> environment variable.
    /// </summary>
    public bool SeedOnSignUp { get; set; } = true;

    /// <summary>
    ///     Days of history generated for a new account, up to the moment of the sign-up. Default: <c>90</c> (3 months).
    /// </summary>
    public int HistoryDays { get; set; } = 90;
}