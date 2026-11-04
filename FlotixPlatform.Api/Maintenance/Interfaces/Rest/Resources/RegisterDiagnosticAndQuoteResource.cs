using System.ComponentModel.DataAnnotations;

namespace FlotixPlatform.Api.Maintenance.Interfaces.Rest.Resources;

/// <summary>
///     Represents the diagnostic and quote registered by the workshop.
/// </summary>
/// <param name="EstimatedCost">The estimated cost of the repair.</param>
public record RegisterDiagnosticAndQuoteResource(
    [Range(0, 1000000, ErrorMessage = "RangeField")]
    decimal EstimatedCost);
