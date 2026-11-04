namespace FlotixPlatform.API.Maintenance.Domain.Model.Commands;

/// <summary>
///     Command to register the diagnostic and quote of a maintenance request
/// </summary>
/// <param name="MaintenanceRequestId">The identifier of the maintenance request.</param>
/// <param name="EstimatedCost">The estimated cost quoted by the workshop.</param>
public record RegisterDiagnosticAndQuoteCommand(Guid MaintenanceRequestId, decimal EstimatedCost);
