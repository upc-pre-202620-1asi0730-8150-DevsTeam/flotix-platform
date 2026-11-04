using FlotixPlatform.Api.Monitoring.Domain.Model.Aggregates;
using FlotixPlatform.Api.Monitoring.Application.Errors;
using FlotixPlatform.Api.Monitoring.Application.Internal.OutboundServices;
using FlotixPlatform.Api.Monitoring.Application.Services;
using FlotixPlatform.Api.Monitoring.Domain.Model.Commands;
using FlotixPlatform.Api.Monitoring.Domain.Repositories;
using FlotixPlatform.Api.Shared.Domain.Repositories;
using FlotixPlatform.Api.Shared.Application.Patterns;
using Microsoft.EntityFrameworkCore;

namespace FlotixPlatform.Api.Monitoring.Application.Internal.CommandServices;

/// <summary>
///     Application service for the ingestion of telemetry data.
/// </summary>
/// <remarks>
///     Corrupt packets and packets of devices not linked to a vehicle are discarded and recorded in the error
///     log. Valid packets are stored, the device is marked as connected and the new location of the vehicle
///     is published in real time.
/// </remarks>
/// <param name="telemetryDataRepository">Repository for accessing telemetry record data.</param>
/// <param name="iotDeviceRepository">Repository for accessing the IoT devices that send the packets.</param>
/// <param name="telemetryDataQueue">Queue that hands the received packets to the IoT Telemetry Processor.</param>
/// <param name="vehicleLocationNotifier">Outbound service that publishes the updated vehicle location.</param>
/// <param name="unitOfWork">Unit of work for managing transaction scope.</param>
/// <param name="logger">Logger for diagnostic and error reporting.</param>
public class TelemetryDataCommandService(
    ITelemetryDataRepository telemetryDataRepository,
    IIotDeviceRepository iotDeviceRepository,
    ITelemetryDataQueue telemetryDataQueue,
    IVehicleLocationNotifier vehicleLocationNotifier,
    IUnitOfWork unitOfWork,
    ILogger<TelemetryDataCommandService> logger)
    : ITelemetryDataCommandService
{
    /// <inheritdoc />
    public async Task<Result<ReceiveTelemetryDataCommand, ReceiveTelemetryDataError>> Handle(ReceiveTelemetryDataCommand command,
        CancellationToken cancellationToken = default)
    {
        try
        {
            TelemetryData.Validate(command);
            await telemetryDataQueue.EnqueueAsync(command, cancellationToken);
            return new Result<ReceiveTelemetryDataCommand, ReceiveTelemetryDataError>.Success(command);
        }
        catch (ArgumentException ex)
        {
            logger.LogError(ex, "Corrupt telemetry packet discarded for device {DeviceId}", command.DeviceId);
            return new Result<ReceiveTelemetryDataCommand, ReceiveTelemetryDataError>.Failure(
                ReceiveTelemetryDataError.InvalidTelemetryData);
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error receiving telemetry packet for device {DeviceId}", command.DeviceId);
            return new Result<ReceiveTelemetryDataCommand, ReceiveTelemetryDataError>.Failure(
                ReceiveTelemetryDataError.UnexpectedError);
        }
    }

    /// <inheritdoc />
    public async Task<Result<TelemetryData, ProcessTelemetryDataError>> Process(ReceiveTelemetryDataCommand command,
        CancellationToken cancellationToken = default)
    {
        var iotDevice = await iotDeviceRepository.FindByIdAsync(command.DeviceId, cancellationToken);
        if (iotDevice is null)
        {
            logger.LogError("Telemetry packet discarded: device {DeviceId} is not linked to a vehicle", command.DeviceId);
            return new Result<TelemetryData, ProcessTelemetryDataError>.Failure(
                ProcessTelemetryDataError.IotDeviceNotFound);
        }

        TelemetryData telemetryData;
        try
        {
            telemetryData = new TelemetryData(command);
            iotDevice.Connect();
            iotDeviceRepository.Update(iotDevice);
            await telemetryDataRepository.AddAsync(telemetryData, cancellationToken);
            await unitOfWork.CompleteAsync(cancellationToken);
        }
        catch (ArgumentException ex)
        {
            logger.LogError(ex, "Corrupt telemetry packet discarded for device {DeviceId}", command.DeviceId);
            return new Result<TelemetryData, ProcessTelemetryDataError>.Failure(
                ProcessTelemetryDataError.InvalidTelemetryData);
        }
        catch (DbUpdateException ex)
        {
            logger.LogError(ex, "Database update failed processing telemetry packet for device {DeviceId}", command.DeviceId);
            return new Result<TelemetryData, ProcessTelemetryDataError>.Failure(
                ProcessTelemetryDataError.UnexpectedError);
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error processing telemetry packet for device {DeviceId}", command.DeviceId);
            return new Result<TelemetryData, ProcessTelemetryDataError>.Failure(
                ProcessTelemetryDataError.UnexpectedError);
        }

        try
        {
            await vehicleLocationNotifier.NotifyAsync(iotDevice.VehicleId, telemetryData, cancellationToken);
        }
        catch (Exception ex) when (ex is not OperationCanceledException)
        {
            logger.LogWarning(ex, "Vehicle location of {VehicleId} stored but not published", iotDevice.VehicleId);
        }

        return new Result<TelemetryData, ProcessTelemetryDataError>.Success(telemetryData);
    }
}
