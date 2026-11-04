using System.Threading.Channels;
using FlotixPlatform.Api.Monitoring.Application.Internal.OutboundServices;
using FlotixPlatform.Api.Monitoring.Domain.Model.Commands;

namespace FlotixPlatform.API.Monitoring.Infrastructure.Queues.Channels;

public class TelemetryDataQueue : ITelemetryDataQueue
{
    private const int Capacity = 10_000;

    private readonly Channel<ReceiveTelemetryDataCommand> _channel =
        Channel.CreateBounded<ReceiveTelemetryDataCommand>(new BoundedChannelOptions(Capacity)
        {
            FullMode = BoundedChannelFullMode.Wait,
            SingleReader = true
        });

    public ValueTask EnqueueAsync(ReceiveTelemetryDataCommand command, CancellationToken cancellationToken = default) =>
        _channel.Writer.WriteAsync(command, cancellationToken);

    public IAsyncEnumerable<ReceiveTelemetryDataCommand> DequeueAllAsync(CancellationToken cancellationToken) =>
        _channel.Reader.ReadAllAsync(cancellationToken);
}
