using FlotixPlatform.API.Maintenance.Domain.Model.Aggregates;
using FlotixPlatform.API.Maintenance.Domain.Model.Commands;
using FlotixPlatform.API.Maintenance.Application.Errors;
using FlotixPlatform.API.Shared.Application.Patterns;

namespace FlotixPlatform.API.Maintenance.Application.Services;

/// <summary>
///     Command service interface for maintenance request operations.
/// </summary>
/// <remarks>
///     This interface defines the contract for the lifecycle of a maintenance request between the owner and
///     the workshop: submission, diagnostic and quote, budget decision, repair start, completion and deletion.
/// </remarks>
public interface IMaintenanceRequestCommandService
{
    /// <summary>
    ///     Handle the submit maintenance request command.
    /// </summary>
    /// <param name="command">SubmitMaintenanceRequestCommand command</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>
    ///     A Result containing the submitted MaintenanceRequest on success, or the error reason on failure.
    /// </returns>
    Task<Result<MaintenanceRequest, SubmitMaintenanceRequestError>> Handle(SubmitMaintenanceRequestCommand command, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Handle the register diagnostic and quote command.
    /// </summary>
    /// <param name="command">RegisterDiagnosticAndQuoteCommand command</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>
    ///     A Result containing the quoted MaintenanceRequest on success, or the error reason on failure.
    /// </returns>
    Task<Result<MaintenanceRequest, ChangeMaintenanceRequestStatusError>> Handle(RegisterDiagnosticAndQuoteCommand command, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Handle the approve or reject budget command.
    /// </summary>
    /// <param name="command">ApproveOrRejectBudgetCommand command</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>
    ///     A Result containing the approved or rejected MaintenanceRequest on success, or the error reason on failure.
    /// </returns>
    Task<Result<MaintenanceRequest, ChangeMaintenanceRequestStatusError>> Handle(ApproveOrRejectBudgetCommand command, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Handle the update repair status command, which starts the approved repair.
    /// </summary>
    /// <param name="command">UpdateRepairStatusCommand command</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>
    ///     A Result containing the MaintenanceRequest in repair on success, or the error reason on failure.
    /// </returns>
    Task<Result<MaintenanceRequest, ChangeMaintenanceRequestStatusError>> Handle(UpdateRepairStatusCommand command, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Handle the complete maintenance command.
    /// </summary>
    /// <param name="command">CompleteMaintenanceCommand command</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>
    ///     A Result containing the completed MaintenanceRequest on success, or the error reason on failure.
    /// </returns>
    Task<Result<MaintenanceRequest, ChangeMaintenanceRequestStatusError>> Handle(CompleteMaintenanceCommand command, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Handle the delete maintenance request command.
    /// </summary>
    /// <param name="command">DeleteMaintenanceRequestCommand command</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>
    ///     A Result containing the deleted MaintenanceRequest on success, or the error reason on failure.
    /// </returns>
    Task<Result<MaintenanceRequest, DeleteMaintenanceRequestError>> Handle(DeleteMaintenanceRequestCommand command, CancellationToken cancellationToken = default);
}
