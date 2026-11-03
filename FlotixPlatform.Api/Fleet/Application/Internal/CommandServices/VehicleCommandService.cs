using FlotixPlatform.Api.Fleet.Domain.Model.Aggregates;
using FlotixPlatform.Api.Fleet.Application.Errors;
using FlotixPlatform.Api.Fleet.Application.Services;
using FlotixPlatform.Api.Fleet.Domain.Model.Commands;
using FlotixPlatform.Api.Fleet.Domain.Repositories;
using FlotixPlatform.Api.Shared.Domain.Repositories;
using FlotixPlatform.Api.Shared.Application.Patterns;
using Microsoft.EntityFrameworkCore;

namespace FlotixPlatform.API.Fleet.Application.Internal.CommandServices;

/// <summary>
///     Application service for handling vehicle commands.
/// </summary>
/// <remarks>
///     This service acts as the command handler for vehicles.
///     It enforces the application rules, delegates the business rules to the aggregate,
///     then coordinates with the unit of work to persist changes.
///     Logs warnings for rejected commands and errors for persistence failures.
/// </remarks>
/// <param name="vehicleRepository">Repository for accessing vehicle data.</param>
/// <param name="unitOfWork">Unit of work for managing transaction scope.</param>
/// <param name="logger">Logger for diagnostic and error reporting.</param>
/// See
/// <see cref="IVehicleRepository">IVehicleRepository</see>
/// ,
/// <see cref="IUnitOfWork">IUnitOfWork</see>
public class VehicleCommandService(
    IVehicleRepository vehicleRepository,
    IUnitOfWork unitOfWork,
    ILogger<VehicleCommandService> logger)
    : IVehicleCommandService
{
    /// <inheritdoc />
    public async Task<Result<Vehicle, CreateVehicleError>> Handle(CreateVehicleCommand command,
        CancellationToken cancellationToken = default)
    {
        if (await vehicleRepository.ExistsByLicensePlateAsync(command.LicensePlate, cancellationToken))
        {
            logger.LogWarning("Duplicate vehicle rejected for LicensePlate {LicensePlate}", command.LicensePlate);
            return new Result<Vehicle, CreateVehicleError>.Failure(
                CreateVehicleError.DuplicateVehicleLicensePlate);
        }

        try
        {
            var vehicle = new Vehicle(command);
            await vehicleRepository.AddAsync(vehicle, cancellationToken);
            await unitOfWork.CompleteAsync(cancellationToken);
            return new Result<Vehicle, CreateVehicleError>.Success(vehicle);
        }
        catch (ArgumentException ex)
        {
            logger.LogWarning(ex, "Invalid arguments while creating vehicle");
            return new Result<Vehicle, CreateVehicleError>.Failure(CreateVehicleError.InvalidVehicle);
        }
        catch (DbUpdateException ex) when (IsDuplicateKeyViolation(ex))
        {
            logger.LogWarning(ex, "Duplicate key violation creating vehicle");
            return new Result<Vehicle, CreateVehicleError>.Failure(CreateVehicleError.DuplicateVehicleLicensePlate);
        }
        catch (DbUpdateException ex) when (IsForeignKeyViolation(ex))
        {
            logger.LogWarning(ex, "Foreign key violation creating vehicle");
            return new Result<Vehicle, CreateVehicleError>.Failure(CreateVehicleError.ReferencedResourceNotFound);
        }
        catch (DbUpdateException ex)
        {
            logger.LogError(ex, "Database update failed creating vehicle");
            return new Result<Vehicle, CreateVehicleError>.Failure(CreateVehicleError.UnexpectedError);
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error creating vehicle");
            return new Result<Vehicle, CreateVehicleError>.Failure(CreateVehicleError.UnexpectedError);
        }
    }

    /// <inheritdoc />
    public async Task<Result<Vehicle, UpdateVehicleError>> Handle(UpdateVehicleCommand command,
        CancellationToken cancellationToken = default)
    {
        var vehicle = await vehicleRepository.FindByIdAsync(command.VehicleId, cancellationToken);
        if (vehicle is null)
            return new Result<Vehicle, UpdateVehicleError>.Failure(
                UpdateVehicleError.VehicleNotFound);

        if (vehicle.LicensePlate != command.LicensePlate &&
            await vehicleRepository.ExistsByLicensePlateAsync(command.LicensePlate, cancellationToken))
        {
            logger.LogWarning("Duplicate vehicle rejected for LicensePlate {LicensePlate}", command.LicensePlate);
            return new Result<Vehicle, UpdateVehicleError>.Failure(
                UpdateVehicleError.DuplicateVehicleLicensePlate);
        }

        try
        {
            vehicle.UpdateInformation(command);
            vehicleRepository.Update(vehicle);
            await unitOfWork.CompleteAsync(cancellationToken);
            return new Result<Vehicle, UpdateVehicleError>.Success(vehicle);
        }
        catch (InvalidOperationException ex)
        {
            logger.LogWarning(ex, "Business rule rejected updating vehicle");
            return new Result<Vehicle, UpdateVehicleError>.Failure(UpdateVehicleError.MileageCannotDecrease);
        }
        catch (ArgumentException ex)
        {
            logger.LogWarning(ex, "Invalid arguments while updating vehicle");
            return new Result<Vehicle, UpdateVehicleError>.Failure(UpdateVehicleError.InvalidVehicle);
        }
        catch (DbUpdateException ex) when (IsDuplicateKeyViolation(ex))
        {
            logger.LogWarning(ex, "Duplicate key violation updating vehicle");
            return new Result<Vehicle, UpdateVehicleError>.Failure(UpdateVehicleError.DuplicateVehicleLicensePlate);
        }
        catch (DbUpdateException ex) when (IsForeignKeyViolation(ex))
        {
            logger.LogWarning(ex, "Foreign key violation updating vehicle");
            return new Result<Vehicle, UpdateVehicleError>.Failure(UpdateVehicleError.ReferencedResourceNotFound);
        }
        catch (DbUpdateException ex)
        {
            logger.LogError(ex, "Database update failed updating vehicle");
            return new Result<Vehicle, UpdateVehicleError>.Failure(UpdateVehicleError.UnexpectedError);
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error updating vehicle");
            return new Result<Vehicle, UpdateVehicleError>.Failure(UpdateVehicleError.UnexpectedError);
        }
    }

    /// <inheritdoc />
    public async Task<Result<Vehicle, DeleteVehicleError>> Handle(DeleteVehicleCommand command,
        CancellationToken cancellationToken = default)
    {
        var vehicle = await vehicleRepository.FindByIdAsync(command.VehicleId, cancellationToken);
        if (vehicle is null)
            return new Result<Vehicle, DeleteVehicleError>.Failure(
                DeleteVehicleError.VehicleNotFound);

        try
        {
            vehicleRepository.Remove(vehicle);
            await unitOfWork.CompleteAsync(cancellationToken);
            return new Result<Vehicle, DeleteVehicleError>.Success(vehicle);
        }
        catch (DbUpdateException ex) when (IsForeignKeyViolation(ex))
        {
            logger.LogWarning(ex, "Foreign key violation deleting vehicle");
            return new Result<Vehicle, DeleteVehicleError>.Failure(DeleteVehicleError.VehicleHasDependents);
        }
        catch (DbUpdateException ex)
        {
            logger.LogError(ex, "Database update failed deleting vehicle");
            return new Result<Vehicle, DeleteVehicleError>.Failure(DeleteVehicleError.UnexpectedError);
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error deleting vehicle");
            return new Result<Vehicle, DeleteVehicleError>.Failure(DeleteVehicleError.UnexpectedError);
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