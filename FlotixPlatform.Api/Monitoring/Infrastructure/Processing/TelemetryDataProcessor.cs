using FlotixPlatform.Api.Monitoring.Application.Internal.OutboundServices;
using FlotixPlatform.Api.Monitoring.Application.Services;

namespace FlotixPlatform.Api.Monitoring.Infrastructure.Processing;

public class TelemetryDataProcessor(
    ITelemetryDataQueue telemetryDataQueue,
    IServiceScopeFactory serviceScopeFactory,
    ILogger<TelemetryDataProcessor> logger)
    : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await foreach (var command in telemetryDataQueue.DequeueAllAsync(stoppingToken))
        {
            try
            {
                await using var scope = serviceScopeFactory.CreateAsyncScope();
                var telemetryDataCommandService = scope.ServiceProvider.GetRequiredService<ITelemetryDataCommandService>();
                await telemetryDataCommandService.Process(command, stoppingToken);
            }
            catch (Exception ex) when (ex is not OperationCanceledException)
            {
                logger.LogError(ex, "Unexpected error in the IoT Telemetry Processor for device {DeviceId}", command.DeviceId);
            }
        }
    }
}
