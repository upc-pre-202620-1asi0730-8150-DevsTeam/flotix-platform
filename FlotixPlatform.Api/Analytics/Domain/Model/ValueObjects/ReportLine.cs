namespace FlotixPlatform.Api.Analytics.Domain.Model.ValueObjects;

/// <summary>
///     Value object representing the figures of a vehicle in a generated report.
/// </summary>
/// <param name="VehicleId">The identifier of the vehicle.</param>
/// <param name="LicensePlate">The license plate of the vehicle.</param>
/// <param name="Records">The number of records of the vehicle in the period (refuels or maintenance requests).</param>
/// <param name="Quantity">The liters loaded (fuel report) or the completed repairs (maintenance report).</param>
/// <param name="TotalCost">The fuel cost (fuel report) or the approved budgets (maintenance report).</param>
public sealed record ReportLine(Guid VehicleId, string LicensePlate, int Records, decimal Quantity, decimal TotalCost);
