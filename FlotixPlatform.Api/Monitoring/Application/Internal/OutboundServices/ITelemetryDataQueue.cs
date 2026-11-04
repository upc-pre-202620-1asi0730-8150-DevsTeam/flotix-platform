using FlotixPlatform.Api.Monitoring.Domain.Model.Commands;

namespace FlotixPlatform.Api.Monitoring.Application.Internal.OutboundServices;

/// <summary>
///     Outbound service contract for the queue that decouples the reception of telemetry packets
///     from their processing by the IoT Telemetry Processor.
/// </summary>
public interface ITelemetryDataQueue
{
    /// <summary>
    ///     Queues a received telemetry packet to be processed in the background.
    /// </summary>
    /// <param name="command">The received telemetry packet.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    ValueTask EnqueueAsync(ReceiveTelemetryDataCommand command, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Reads the queued telemetry packets as they arrive.
    /// </summary>
    /// <param name="cancellationToken">Token to stop reading.</param>
    /// <returns>The queued telemetry packets.</returns>
    IAsyncEnumerable<ReceiveTelemetryDataCommand> DequeueAllAsync(CancellationToken cancellationToken);
}
