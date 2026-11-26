namespace FlotixPlatform.Api.Profiles.Domain.Model.ValueObjects;

/// <summary>
///     Subscription plans offered on the Flotix landing page.
/// </summary>
public enum EProfilePlan
{
    Starter,
    Growth,
    Fleet
}

/// <summary>
///     Pricing and limits of each <see cref="EProfilePlan" />. The server is the source of truth, so the price
///     is never taken from the client.
/// </summary>
public static class EProfilePlanExtensions
{
    /// <summary>Monthly price in Peruvian soles (S/).</summary>
    public static decimal MonthlyPrice(this EProfilePlan plan)
    {
        return plan switch
        {
            EProfilePlan.Starter => 149m,
            EProfilePlan.Growth => 349m,
            EProfilePlan.Fleet => 749m,
            _ => throw new ArgumentOutOfRangeException(nameof(plan), plan, null)
        };
    }

    /// <summary>Maximum number of vehicles, or <c>null</c> when unlimited.</summary>
    public static int? VehicleLimit(this EProfilePlan plan)
    {
        return plan switch
        {
            EProfilePlan.Starter => 5,
            EProfilePlan.Growth => 25,
            EProfilePlan.Fleet => null,
            _ => throw new ArgumentOutOfRangeException(nameof(plan), plan, null)
        };
    }
}
