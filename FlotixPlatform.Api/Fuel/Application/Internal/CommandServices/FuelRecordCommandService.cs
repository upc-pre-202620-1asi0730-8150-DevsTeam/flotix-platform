using FlotixPlatform.Api.Fuel.Domain.Model.Aggregates;
using FlotixPlatform.Api.Fuel.Application.Errors;
using FlotixPlatform.Api.Fuel.Application.Services;
using FlotixPlatform.Api.Fuel.Domain.Model.Commands;
using FlotixPlatform.Api.Fuel.Domain.Repositories;
using FlotixPlatform.Api.Shared.Domain.Repositories;
using FlotixPlatform.Api.Shared.Application.Patterns;
using Microsoft.EntityFrameworkCore;

namespace FlotixPlatform.Api.Fuel.Application.Internal.CommandServices;

/// <summary>
///     Application service for handling fuel record commands.
/// </summary>
/// <remarks>
///     This service acts as the command handler for fuel records.
///     It enforces the application rules, delegates the business rules to the aggregate,
///     then coordinates with the unit of work to persist changes.
///     Logs warnings for rejected commands and errors for persistence failures.
/// </remarks>
/// <param name="fuelRecordRepository">Repository for accessing fuel record data.</param>
/// <param name="unitOfWork">Unit of work for managing transaction scope.</param>
/// <param name="logger">Logger for diagnostic and error reporting.</param>
/// See
/// <see cref="IFuelRecordRepository">IFuelRecordRepository</see>
/// ,
/// <see cref="IUnitOfWork">IUnitOfWork</see>
public class FuelRecordCommandService(
    IFuelRecordRepository fuelRecordRepository,
    IUnitOfWork unitOfWork,
    ILogger<FuelRecordCommandService> logger)
    : IFuelRecordCommandService
{
    /// <inheritdoc />
    public async Task<Result<FuelRecord, CreateFuelRecordError>> Handle(CreateFuelRecordCommand command,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var fuelRecord = new FuelRecord(command);
            await fuelRecordRepository.AddAsync(fuelRecord, cancellationToken);
            await unitOfWork.CompleteAsync(cancellationToken);
            return new Result<FuelRecord, CreateFuelRecordError>.Success(fuelRecord);
        }
        catch (ArgumentException ex)
        {
            logger.LogWarning(ex, "Invalid arguments while creating fuel record");
            return new Result<FuelRecord, CreateFuelRecordError>.Failure(CreateFuelRecordError.InvalidFuelRecord);
        }
        catch (DbUpdateException ex) when (IsForeignKeyViolation(ex))
        {
            logger.LogWarning(ex, "Foreign key violation creating fuel record");
            return new Result<FuelRecord, CreateFuelRecordError>.Failure(CreateFuelRecordError.ReferencedResourceNotFound);
        }
        catch (DbUpdateException ex)
        {
            logger.LogError(ex, "Database update failed creating fuel record");
            return new Result<FuelRecord, CreateFuelRecordError>.Failure(CreateFuelRecordError.UnexpectedError);
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error creating fuel record");
            return new Result<FuelRecord, CreateFuelRecordError>.Failure(CreateFuelRecordError.UnexpectedError);
        }
    }

    /// <inheritdoc />
    public async Task<Result<FuelRecord, UpdateFuelRecordError>> Handle(UpdateFuelRecordCommand command,
        CancellationToken cancellationToken = default)
    {
        var fuelRecord = await fuelRecordRepository.FindByIdAsync(command.FuelRecordId, cancellationToken);
        if (fuelRecord is null)
            return new Result<FuelRecord, UpdateFuelRecordError>.Failure(
                UpdateFuelRecordError.FuelRecordNotFound);

        try
        {
            fuelRecord.UpdateInformation(command);
            fuelRecordRepository.Update(fuelRecord);
            await unitOfWork.CompleteAsync(cancellationToken);
            return new Result<FuelRecord, UpdateFuelRecordError>.Success(fuelRecord);
        }
        catch (ArgumentException ex)
        {
            logger.LogWarning(ex, "Invalid arguments while updating fuel record");
            return new Result<FuelRecord, UpdateFuelRecordError>.Failure(UpdateFuelRecordError.InvalidFuelRecord);
        }
        catch (DbUpdateException ex)
        {
            logger.LogError(ex, "Database update failed updating fuel record");
            return new Result<FuelRecord, UpdateFuelRecordError>.Failure(UpdateFuelRecordError.UnexpectedError);
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error updating fuel record");
            return new Result<FuelRecord, UpdateFuelRecordError>.Failure(UpdateFuelRecordError.UnexpectedError);
        }
    }

    /// <inheritdoc />
    public async Task<Result<FuelRecord, DeleteFuelRecordError>> Handle(DeleteFuelRecordCommand command,
        CancellationToken cancellationToken = default)
    {
        var fuelRecord = await fuelRecordRepository.FindByIdAsync(command.FuelRecordId, cancellationToken);
        if (fuelRecord is null)
            return new Result<FuelRecord, DeleteFuelRecordError>.Failure(
                DeleteFuelRecordError.FuelRecordNotFound);

        try
        {
            fuelRecordRepository.Remove(fuelRecord);
            await unitOfWork.CompleteAsync(cancellationToken);
            return new Result<FuelRecord, DeleteFuelRecordError>.Success(fuelRecord);
        }
        catch (DbUpdateException ex)
        {
            logger.LogError(ex, "Database update failed deleting fuel record");
            return new Result<FuelRecord, DeleteFuelRecordError>.Failure(DeleteFuelRecordError.UnexpectedError);
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error deleting fuel record");
            return new Result<FuelRecord, DeleteFuelRecordError>.Failure(DeleteFuelRecordError.UnexpectedError);
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
