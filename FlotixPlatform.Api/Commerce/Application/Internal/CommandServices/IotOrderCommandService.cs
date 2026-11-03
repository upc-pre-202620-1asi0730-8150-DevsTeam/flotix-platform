using FlotixPlatform.API.Commerce.Domain.Model.Aggregates;
using FlotixPlatform.API.Commerce.Application.Errors;
using FlotixPlatform.API.Commerce.Application.Services;
using FlotixPlatform.API.Commerce.Domain.Model.Commands;
using FlotixPlatform.API.Commerce.Domain.Repositories;
using FlotixPlatform.API.Shared.Domain.Repositories;
using FlotixPlatform.API.Shared.Application.Patterns;
using Microsoft.EntityFrameworkCore;

namespace FlotixPlatform.API.Commerce.Application.Internal.CommandServices;

/// <summary>
///     Application service for handling IoT order commands.
/// </summary>
/// <remarks>
///     This service acts as the command handler for IoT orders.
///     It enforces the application rules, delegates the business rules to the aggregate,
///     then coordinates with the unit of work to persist changes.
///     Logs warnings for rejected commands and errors for persistence failures.
/// </remarks>
/// <param name="iotOrderRepository">Repository for accessing IoT order data.</param>
/// <param name="unitOfWork">Unit of work for managing transaction scope.</param>
/// <param name="logger">Logger for diagnostic and error reporting.</param>
/// See
/// <see cref="IIotOrderRepository">IIotOrderRepository</see>
/// ,
/// <see cref="IUnitOfWork">IUnitOfWork</see>
public class IotOrderCommandService(
    IIotOrderRepository iotOrderRepository,
    IUnitOfWork unitOfWork,
    ILogger<IotOrderCommandService> logger)
    : IIotOrderCommandService
{
    /// <inheritdoc />
    public async Task<Result<IotOrder, CreateIotOrderError>> Handle(CreateIotOrderCommand command,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var iotOrder = new IotOrder(command);
            await iotOrderRepository.AddAsync(iotOrder, cancellationToken);
            await unitOfWork.CompleteAsync(cancellationToken);
            return new Result<IotOrder, CreateIotOrderError>.Success(iotOrder);
        }
        catch (ArgumentException ex)
        {
            logger.LogWarning(ex, "Invalid arguments while creating IoT order");
            return new Result<IotOrder, CreateIotOrderError>.Failure(CreateIotOrderError.InvalidIotOrder);
        }
        catch (DbUpdateException ex) when (IsForeignKeyViolation(ex))
        {
            logger.LogWarning(ex, "Foreign key violation creating IoT order");
            return new Result<IotOrder, CreateIotOrderError>.Failure(CreateIotOrderError.ReferencedResourceNotFound);
        }
        catch (DbUpdateException ex)
        {
            logger.LogError(ex, "Database update failed creating IoT order");
            return new Result<IotOrder, CreateIotOrderError>.Failure(CreateIotOrderError.UnexpectedError);
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error creating IoT order");
            return new Result<IotOrder, CreateIotOrderError>.Failure(CreateIotOrderError.UnexpectedError);
        }
    }

    /// <inheritdoc />
    public async Task<Result<IotOrder, UpdateIotOrderError>> Handle(UpdateIotOrderCommand command,
        CancellationToken cancellationToken = default)
    {
        var iotOrder = await iotOrderRepository.FindByIdAsync(command.IotOrderId, cancellationToken);
        if (iotOrder is null)
            return new Result<IotOrder, UpdateIotOrderError>.Failure(
                UpdateIotOrderError.IotOrderNotFound);

        try
        {
            iotOrder.UpdateInformation(command);
            iotOrderRepository.Update(iotOrder);
            await unitOfWork.CompleteAsync(cancellationToken);
            return new Result<IotOrder, UpdateIotOrderError>.Success(iotOrder);
        }
        catch (InvalidOperationException ex)
        {
            logger.LogWarning(ex, "Business rule rejected updating IoT order");
            return new Result<IotOrder, UpdateIotOrderError>.Failure(UpdateIotOrderError.IotOrderAlreadyClosed);
        }
        catch (ArgumentException ex)
        {
            logger.LogWarning(ex, "Invalid arguments while updating IoT order");
            return new Result<IotOrder, UpdateIotOrderError>.Failure(UpdateIotOrderError.InvalidIotOrder);
        }
        catch (DbUpdateException ex)
        {
            logger.LogError(ex, "Database update failed updating IoT order");
            return new Result<IotOrder, UpdateIotOrderError>.Failure(UpdateIotOrderError.UnexpectedError);
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error updating IoT order");
            return new Result<IotOrder, UpdateIotOrderError>.Failure(UpdateIotOrderError.UnexpectedError);
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
