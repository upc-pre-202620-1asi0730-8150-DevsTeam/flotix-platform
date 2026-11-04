using FlotixPlatform.Api.Monitoring.Domain.Model.Aggregates;
using FlotixPlatform.Api.Monitoring.Domain.Model.Commands;
using FlotixPlatform.Api.Monitoring.Application.Errors;
using FlotixPlatform.Api.Shared.Application.Patterns;

namespace FlotixPlatform.Api.Monitoring.Application.Services;

/// <summary>
///     Command service interface for the ingestion of telemetry data.
/// </summary>
/// <remarks>
///     Telemetry packets are received and validated synchronously, then processed in the background by the
///     IoT Telemetry Processor, which persists them and updates the location of the vehicle.
/// </remarks>
public interface ITelemetryDataCommandService
{
    /// <summary>
    ///     Handle the receive telemetry data command: validates the packet and queues it for processing.
    /// </summary>
    /// <param name="command">ReceiveTelemetryDataCommand command</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>
    ///     A Result containing the queued packet on success, or the error reason on failure.
    /// </returns>
    Task<Result<ReceiveTelemetryDataCommand, ReceiveTelemetryDataError>> Handle(ReceiveTelemetryDataCommand command, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Processes a queued telemetry packet: stores the reading, connects the device and publishes the
    ///     updated location of its vehicle.
    /// </summary>
    /// <param name="command">ReceiveTelemetryDataCommand command taken from the queue</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>
    ///     A Result containing the stored TelemetryData on success, or the error reason on failure.
    /// </returns>
    Task<Result<TelemetryData, ProcessTelemetryDataError>> Process(ReceiveTelemetryDataCommand command, CancellationToken cancellationToken = default);
}
