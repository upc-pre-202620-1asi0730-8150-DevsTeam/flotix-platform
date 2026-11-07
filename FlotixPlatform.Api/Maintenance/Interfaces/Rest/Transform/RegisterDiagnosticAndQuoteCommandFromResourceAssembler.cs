using FlotixPlatform.Api.Maintenance.Domain.Model.Commands;
using FlotixPlatform.Api.Maintenance.Interfaces.Rest.Resources;

namespace FlotixPlatform.Api.Maintenance.Interfaces.Rest.Transform;

/// <summary>
///     Assembles a RegisterDiagnosticAndQuoteCommand from a RegisterDiagnosticAndQuoteResource.
/// </summary>
public static class RegisterDiagnosticAndQuoteCommandFromResourceAssembler
{
    /// <summary>
    ///     Assembles a RegisterDiagnosticAndQuoteCommand from a RegisterDiagnosticAndQuoteResource.
    /// </summary>
    /// <param name="maintenanceRequestId">The identifier of the maintenance request</param>
    /// <param name="resource">The RegisterDiagnosticAndQuoteResource resource</param>
    /// <returns>
    ///     A RegisterDiagnosticAndQuoteCommand assembled from the RegisterDiagnosticAndQuoteResource
    /// </returns>
    public static RegisterDiagnosticAndQuoteCommand ToCommandFromResource(Guid maintenanceRequestId,
        RegisterDiagnosticAndQuoteResource resource) =>
        new(maintenanceRequestId, resource.EstimatedCost);
}
