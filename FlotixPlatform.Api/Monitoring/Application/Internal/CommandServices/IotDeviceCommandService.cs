using FlotixPlatform.Api.Monitoring.Domain.Model.Aggregates;
using FlotixPlatform.Api.Monitoring.Application.Errors;
using FlotixPlatform.Api.Monitoring.Application.Services;
using FlotixPlatform.Api.Monitoring.Domain.Model.Commands;
using FlotixPlatform.Api.Monitoring.Domain.Repositories;
using FlotixPlatform.Api.Shared.Domain.Repositories;
using FlotixPlatform.Api.Shared.Application.Patterns;
using Microsoft.EntityFrameworkCore;

namespace FlotixPlatform.Api.Monitoring.Application.Internal.CommandServices;

/// <summary>
///     Application service for handling IoT device commands.
/// </summary>
/// <remarks>
///     This service acts as the command handler for IoT devices.
///     It enforces the application rules, delegates the business rules to the aggregate,
///     then coordinates with the unit of work to persist changes.
///     Logs warnings for rejected commands and errors for persistence failures.
/// </remarks>
/// <param name="iotDeviceRepository">Repository for accessing IoT device data.</param>
/// <param name="unitOfWork">Unit of work for managing transaction scope.</param>
/// <param name="logger">Logger for diagnostic and error reporting.</param>
/// See
/// <see cref="IIotDeviceRepository">IIotDeviceRepository</see>
/// ,
/// <see cref="IUnitOfWork">IUnitOfWork</see>
public class IotDeviceCommandService(
    IIotDeviceRepository iotDeviceRepository,
    IUnitOfWork unitOfWork,
    ILogger<IotDeviceCommandService> logger)
    : IIotDeviceCommandService
{
    /// <inheritdoc />
    public async Task<Result<IotDevice, CreateIotDeviceError>> Handle(CreateIotDeviceCommand command,
        CancellationToken cancellationToken = default)
    {
        if (await iotDeviceRepository.ExistsByVehicleIdAsync(command.VehicleId, cancellationToken))
        {
            logger.LogWarning("Duplicate IoT device rejected for VehicleId {VehicleId}", command.VehicleId);
            return new Result<IotDevice, CreateIotDeviceError>.Failure(
                CreateIotDeviceError.DuplicateIotDeviceVehicleId);
        }

        if (await iotDeviceRepository.ExistsBySerialNumberAsync(command.SerialNumber, cancellationToken))
        {
            logger.LogWarning("Duplicate IoT device rejected for SerialNumber {SerialNumber}", command.SerialNumber);
            return new Result<IotDevice, CreateIotDeviceError>.Failure(
                CreateIotDeviceError.DuplicateIotDeviceSerialNumber);
        }

        try
        {
            var iotDevice = new IotDevice(command);
            await iotDeviceRepository.AddAsync(iotDevice, cancellationToken);
            await unitOfWork.CompleteAsync(cancellationToken);
            return new Result<IotDevice, CreateIotDeviceError>.Success(iotDevice);
        }
        catch (ArgumentException ex)
        {
            logger.LogWarning(ex, "Invalid arguments while creating IoT device");
            return new Result<IotDevice, CreateIotDeviceError>.Failure(CreateIotDeviceError.InvalidIotDevice);
        }
        catch (DbUpdateException ex) when (IsDuplicateKeyViolation(ex))
        {
            logger.LogWarning(ex, "Duplicate key violation creating IoT device");
            return new Result<IotDevice, CreateIotDeviceError>.Failure(CreateIotDeviceError.DuplicateIotDeviceVehicleId);
        }
        catch (DbUpdateException ex) when (IsForeignKeyViolation(ex))
        {
            logger.LogWarning(ex, "Foreign key violation creating IoT device");
            return new Result<IotDevice, CreateIotDeviceError>.Failure(CreateIotDeviceError.ReferencedResourceNotFound);
        }
        catch (DbUpdateException ex)
        {
            logger.LogError(ex, "Database update failed creating IoT device");
            return new Result<IotDevice, CreateIotDeviceError>.Failure(CreateIotDeviceError.UnexpectedError);
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error creating IoT device");
            return new Result<IotDevice, CreateIotDeviceError>.Failure(CreateIotDeviceError.UnexpectedError);
        }
    }

    /// <inheritdoc />
    public async Task<Result<IotDevice, UpdateIotDeviceError>> Handle(UpdateIotDeviceCommand command,
        CancellationToken cancellationToken = default)
    {
        var iotDevice = await iotDeviceRepository.FindByIdAsync(command.IotDeviceId, cancellationToken);
        if (iotDevice is null)
            return new Result<IotDevice, UpdateIotDeviceError>.Failure(
                UpdateIotDeviceError.IotDeviceNotFound);

        if (iotDevice.VehicleId != command.VehicleId &&
            await iotDeviceRepository.ExistsByVehicleIdAsync(command.VehicleId, cancellationToken))
        {
            logger.LogWarning("Duplicate IoT device rejected for VehicleId {VehicleId}", command.VehicleId);
            return new Result<IotDevice, UpdateIotDeviceError>.Failure(
                UpdateIotDeviceError.DuplicateIotDeviceVehicleId);
        }

        try
        {
            iotDevice.UpdateInformation(command);
            iotDeviceRepository.Update(iotDevice);
            await unitOfWork.CompleteAsync(cancellationToken);
            return new Result<IotDevice, UpdateIotDeviceError>.Success(iotDevice);
        }
        catch (ArgumentException ex)
        {
            logger.LogWarning(ex, "Invalid arguments while updating IoT device");
            return new Result<IotDevice, UpdateIotDeviceError>.Failure(UpdateIotDeviceError.InvalidIotDevice);
        }
        catch (DbUpdateException ex) when (IsDuplicateKeyViolation(ex))
        {
            logger.LogWarning(ex, "Duplicate key violation updating IoT device");
            return new Result<IotDevice, UpdateIotDeviceError>.Failure(UpdateIotDeviceError.DuplicateIotDeviceVehicleId);
        }
        catch (DbUpdateException ex) when (IsForeignKeyViolation(ex))
        {
            logger.LogWarning(ex, "Foreign key violation updating IoT device");
            return new Result<IotDevice, UpdateIotDeviceError>.Failure(UpdateIotDeviceError.ReferencedResourceNotFound);
        }
        catch (DbUpdateException ex)
        {
            logger.LogError(ex, "Database update failed updating IoT device");
            return new Result<IotDevice, UpdateIotDeviceError>.Failure(UpdateIotDeviceError.UnexpectedError);
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error updating IoT device");
            return new Result<IotDevice, UpdateIotDeviceError>.Failure(UpdateIotDeviceError.UnexpectedError);
        }
    }

    /// <inheritdoc />
    public async Task<Result<IotDevice, DeleteIotDeviceError>> Handle(DeleteIotDeviceCommand command,
        CancellationToken cancellationToken = default)
    {
        var iotDevice = await iotDeviceRepository.FindByIdAsync(command.IotDeviceId, cancellationToken);
        if (iotDevice is null)
            return new Result<IotDevice, DeleteIotDeviceError>.Failure(
                DeleteIotDeviceError.IotDeviceNotFound);

        try
        {
            iotDeviceRepository.Remove(iotDevice);
            await unitOfWork.CompleteAsync(cancellationToken);
            return new Result<IotDevice, DeleteIotDeviceError>.Success(iotDevice);
        }
        catch (DbUpdateException ex) when (IsForeignKeyViolation(ex))
        {
            logger.LogWarning(ex, "Foreign key violation deleting IoT device");
            return new Result<IotDevice, DeleteIotDeviceError>.Failure(DeleteIotDeviceError.IotDeviceHasDependents);
        }
        catch (DbUpdateException ex)
        {
            logger.LogError(ex, "Database update failed deleting IoT device");
            return new Result<IotDevice, DeleteIotDeviceError>.Failure(DeleteIotDeviceError.UnexpectedError);
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error deleting IoT device");
            return new Result<IotDevice, DeleteIotDeviceError>.Failure(DeleteIotDeviceError.UnexpectedError);
        }
    }

    /// <summary>
    /// Determines whether a DbUpdateException represents a duplicate key constraint violation.
    /// </summary>
    /// <param name="exception">The exception to inspect.</param>
    /// <returns>True if the exception is due to a PostgreSQL unique violation (SQLSTATE 23505), false otherwise.</returns>
    private static bool IsDuplicateKeyViolation(DbUpdateException exception)
    {
        for (Exception? current = exception; current is not null; current = current.InnerException)
        {
            if (!string.Equals(current.GetType().Name, "PostgresException", StringComparison.Ordinal)) continue;
            var sqlStateProperty = current.GetType().GetProperty("SqlState");
            if (sqlStateProperty?.GetValue(current) is "23505")
                return true;
        }
        return false;
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
