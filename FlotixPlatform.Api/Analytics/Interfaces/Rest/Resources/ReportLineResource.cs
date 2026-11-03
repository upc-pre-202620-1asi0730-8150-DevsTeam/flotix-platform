namespace FlotixPlatform.Api.Analytics.Interfaces.Rest.Resources;

/// <summary>
///     Represents the figures of a vehicle in a generated report.
/// </summary>
/// <param name="VehicleId">The identifier of the vehicle</param>
/// <param name="LicensePlate">The license plate of the vehicle</param>
/// <param name="Records">The number of refuels (fuel report) or maintenance requests (maintenance report) in the period</param>
/// <param name="Quantity">The liters loaded (fuel report) or the completed repairs (maintenance report)</param>
/// <param name="TotalCost">The fuel cost (fuel report) or the approved budgets (maintenance report)</param>
public record ReportLineResource(
    Guid VehicleId,
    string LicensePlate,
    int Records,
    decimal Quantity,
    decimal TotalCost);
