using Microsoft.AspNetCore.SignalR;

namespace FlotixPlatform.Api.Monitoring.Interfaces.Hubs;

public class MonitoringHub : Hub<IMonitoringClient>;
