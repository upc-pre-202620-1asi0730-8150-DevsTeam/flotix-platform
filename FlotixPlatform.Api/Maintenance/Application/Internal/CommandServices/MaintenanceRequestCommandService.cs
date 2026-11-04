using FlotixPlatform.API.Maintenance.Domain.Model.Aggregates;
using FlotixPlatform.API.Maintenance.Application.Errors;
using FlotixPlatform.API.Maintenance.Application.Services;
using FlotixPlatform.API.Maintenance.Domain.Model.Commands;
using FlotixPlatform.API.Maintenance.Domain.Repositories;
using FlotixPlatform.API.Shared.Domain.Repositories;
using FlotixPlatform.API.Shared.Application.Patterns;
using Microsoft.EntityFrameworkCore;

namespace FlotixPlatform.API.Maintenance.Application.Internal.CommandServices;

/// <summary>
///     Application service for handling maintenance request commands.
/// </summary>
/// <remarks>
///     This service acts as the command handler for the lifecycle of maintenance requests.
///     It enforces the application rules, delegates the business rules to the aggregate,
///     then coordinates with the unit of work to persist changes.
///     Logs warnings for rejected commands and errors for persistence failures.
/// </remarks>
/// <param name="maintenanceRequestRepository">Repository for accessing maintenance request data.</param>
/// <param name="unitOfWork">Unit of work for managing transaction scope.</param>
/// <param name="logger">Logger for diagnostic and error reporting.</param>
/// See
/// <see cref="IMaintenanceRequestRepository">IMaintenanceRequestRepository</see>
/// ,
/// <see cref="IUnitOfWork">IUnitOfWork</see>
public class MaintenanceRequestCommandService(
    IMaintenanceRequestRepository maintenanceRequestRepository,
    IUnitOfWork unitOfWork,
    ILogger<MaintenanceRequestCommandService> logger)
    : IMaintenanceRequestCommandService
{
    /// <inheritdoc />
    public async Task<Result<MaintenanceRequest, SubmitMaintenanceRequestError>> Handle(SubmitMaintenanceRequestCommand command,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var maintenanceRequest = new MaintenanceRequest(command);
            await maintenanceRequestRepository.AddAsync(maintenanceRequest, cancellationToken);
            await unitOfWork.CompleteAsync(cancellationToken);
            return new Result<MaintenanceRequest, SubmitMaintenanceRequestError>.Success(maintenanceRequest);
        }
        catch (ArgumentException ex)
        {
            logger.LogWarning(ex, "Invalid arguments while submitting maintenance request");
            return new Result<MaintenanceRequest, SubmitMaintenanceRequestError>.Failure(SubmitMaintenanceRequestError.InvalidMaintenanceRequest);
        }
        catch (DbUpdateException ex) when (IsForeignKeyViolation(ex))
        {
            logger.LogWarning(ex, "Foreign key violation submitting maintenance request");
            return new Result<MaintenanceRequest, SubmitMaintenanceRequestError>.Failure(SubmitMaintenanceRequestError.ReferencedResourceNotFound);
        }
        catch (DbUpdateException ex)
        {
            logger.LogError(ex, "Database update failed submitting maintenance request");
            return new Result<MaintenanceRequest, SubmitMaintenanceRequestError>.Failure(SubmitMaintenanceRequestError.UnexpectedError);
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error submitting maintenance request");
            return new Result<MaintenanceRequest, SubmitMaintenanceRequestError>.Failure(SubmitMaintenanceRequestError.UnexpectedError);
        }
    }

    /// <inheritdoc />
    public Task<Result<MaintenanceRequest, ChangeMaintenanceRequestStatusError>> Handle(RegisterDiagnosticAndQuoteCommand command,
        CancellationToken cancellationToken = default) =>
        ChangeStatusAsync(command.MaintenanceRequestId, request => request.SubmitQuote(command.EstimatedCost), cancellationToken);

    /// <inheritdoc />
    public Task<Result<MaintenanceRequest, ChangeMaintenanceRequestStatusError>> Handle(ApproveOrRejectBudgetCommand command,
        CancellationToken cancellationToken = default) =>
        ChangeStatusAsync(command.MaintenanceRequestId, request =>
        {
            if (command.IsApproved) request.Approve();
            else request.Reject();
        }, cancellationToken);

    /// <inheritdoc />
    public Task<Result<MaintenanceRequest, ChangeMaintenanceRequestStatusError>> Handle(UpdateRepairStatusCommand command,
        CancellationToken cancellationToken = default) =>
        ChangeStatusAsync(command.MaintenanceRequestId, request => request.StartRepair(), cancellationToken);

    /// <inheritdoc />
    public Task<Result<MaintenanceRequest, ChangeMaintenanceRequestStatusError>> Handle(CompleteMaintenanceCommand command,
        CancellationToken cancellationToken = default) =>
        ChangeStatusAsync(command.MaintenanceRequestId, request => request.Finish(), cancellationToken);

    /// <inheritdoc />
    public async Task<Result<MaintenanceRequest, DeleteMaintenanceRequestError>> Handle(DeleteMaintenanceRequestCommand command,
        CancellationToken cancellationToken = default)
    {
        var maintenanceRequest = await maintenanceRequestRepository.FindByIdAsync(command.MaintenanceRequestId, cancellationToken);
        if (maintenanceRequest is null)
            return new Result<MaintenanceRequest, DeleteMaintenanceRequestError>.Failure(
                DeleteMaintenanceRequestError.MaintenanceRequestNotFound);

        try
        {
            maintenanceRequestRepository.Remove(maintenanceRequest);
            await unitOfWork.CompleteAsync(cancellationToken);
            return new Result<MaintenanceRequest, DeleteMaintenanceRequestError>.Success(maintenanceRequest);
        }
        catch (DbUpdateException ex)
        {
            logger.LogError(ex, "Database update failed deleting maintenance request");
            return new Result<MaintenanceRequest, DeleteMaintenanceRequestError>.Failure(DeleteMaintenanceRequestError.UnexpectedError);
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error deleting maintenance request");
            return new Result<MaintenanceRequest, DeleteMaintenanceRequestError>.Failure(DeleteMaintenanceRequestError.UnexpectedError);
        }
    }

    /// <summary>
    ///     Applies a lifecycle transition to a maintenance request and persists it.
    /// </summary>
    /// <param name="maintenanceRequestId">The identifier of the maintenance request.</param>
    /// <param name="transition">The aggregate behavior that performs the transition.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>A Result containing the updated MaintenanceRequest on success, or the error reason on failure.</returns>
    private async Task<Result<MaintenanceRequest, ChangeMaintenanceRequestStatusError>> ChangeStatusAsync(
        Guid maintenanceRequestId,
        Action<MaintenanceRequest> transition,
        CancellationToken cancellationToken)
    {
        var maintenanceRequest = await maintenanceRequestRepository.FindByIdAsync(maintenanceRequestId, cancellationToken);
        if (maintenanceRequest is null)
            return new Result<MaintenanceRequest, ChangeMaintenanceRequestStatusError>.Failure(
                ChangeMaintenanceRequestStatusError.MaintenanceRequestNotFound);

        try
        {
            transition(maintenanceRequest);
            maintenanceRequestRepository.Update(maintenanceRequest);
            await unitOfWork.CompleteAsync(cancellationToken);
            return new Result<MaintenanceRequest, ChangeMaintenanceRequestStatusError>.Success(maintenanceRequest);
        }
        catch (InvalidOperationException ex)
        {
            logger.LogWarning(ex, "Invalid status transition for maintenance request {MaintenanceRequestId}", maintenanceRequestId);
            return new Result<MaintenanceRequest, ChangeMaintenanceRequestStatusError>.Failure(
                ChangeMaintenanceRequestStatusError.InvalidStatusTransition);
        }
        catch (ArgumentException ex)
        {
            logger.LogWarning(ex, "Invalid arguments while changing maintenance request {MaintenanceRequestId}", maintenanceRequestId);
            return new Result<MaintenanceRequest, ChangeMaintenanceRequestStatusError>.Failure(
                ChangeMaintenanceRequestStatusError.InvalidMaintenanceRequest);
        }
        catch (DbUpdateException ex)
        {
            logger.LogError(ex, "Database update failed changing maintenance request {MaintenanceRequestId}", maintenanceRequestId);
            return new Result<MaintenanceRequest, ChangeMaintenanceRequestStatusError>.Failure(
                ChangeMaintenanceRequestStatusError.UnexpectedError);
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error changing maintenance request {MaintenanceRequestId}", maintenanceRequestId);
            return new Result<MaintenanceRequest, ChangeMaintenanceRequestStatusError>.Failure(
                ChangeMaintenanceRequestStatusError.UnexpectedError);
        }
    }

    /// <summary>
    /// Determines whether a DbUpdateException represents a foreign key constraint violation.
    /// </summary>
    /// <param name="exception">The exception to inspect.</param>
    /// <returns>True if the exception is due to a PostgreSQL foreign key violation (SQLSTATE 23503), false otherwise.</returns>
    private static bool IsForeignKeyViolation(DbUpdateException exception)
    {
        for (Exception? current = exception; current is not null; current = current.InnerException)
        {
            if (!string.Equals(current.GetType().Name, "PostgresException", StringComparison.Ordinal)) continue;
            var sqlStateProperty = current.GetType().GetProperty("SqlState");
            if (sqlStateProperty?.GetValue(current) is "23503")
                return true;
        }
        return false;
    }
}
