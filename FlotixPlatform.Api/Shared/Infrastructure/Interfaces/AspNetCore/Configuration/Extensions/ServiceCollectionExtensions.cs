using System.Text.Json.Serialization;
using FlotixPlatform.Api.DemoData.Application.Services;
using FlotixPlatform.Api.DemoData.Infrastructure.Configuration;
using FlotixPlatform.Api.DemoData.Infrastructure.Persistence.EntityFrameworkCore.Services;
using FlotixPlatform.Api.Iam.Application.Acl;
using FlotixPlatform.Api.Iam.Application.CommandServices;
using FlotixPlatform.Api.Iam.Application.Internal.CommandServices;
using FlotixPlatform.Api.Iam.Application.Internal.OutboundServices;
using FlotixPlatform.Api.Iam.Application.Internal.QueryServices;
using FlotixPlatform.Api.Iam.Application.QueryServices;
using FlotixPlatform.Api.Iam.Domain.Repositories;
using FlotixPlatform.Api.Iam.Infrastructure.Hashing.BCrypt.Services;
using FlotixPlatform.Api.Iam.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using FlotixPlatform.Api.Iam.Infrastructure.Tokens.Jwt.Configuration;
using FlotixPlatform.Api.Iam.Infrastructure.Tokens.Jwt.Services;
using FlotixPlatform.Api.Iam.Interfaces.Acl;
using FlotixPlatform.Api.Monitoring.Application.Internal.OutboundServices;
using FlotixPlatform.Api.Monitoring.Infrastructure.Processing;
using FlotixPlatform.Api.Monitoring.Infrastructure.Queues.Channels;
using FlotixPlatform.Api.Monitoring.Infrastructure.Realtime.SignalR.Services;
using FlotixPlatform.Api.Shared.Domain.Repositories;
using FlotixPlatform.Api.Shared.Infrastructure.Interfaces.AspNetCore.Configuration;
using FlotixPlatform.Api.Shared.Infrastructure.Interfaces.AspNetCore.OpenApi;
using FlotixPlatform.Api.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using FlotixPlatform.Api.Shared.Interfaces.Rest.ProblemDetails;
using AnalyticsOpenApi = FlotixPlatform.Api.Analytics.Interfaces.Rest.Transform.LocalizedOpenApiAssembler;
using CommerceOpenApi = FlotixPlatform.Api.Commerce.Interfaces.Rest.Transform.LocalizedOpenApiAssembler;
using FleetOpenApi = FlotixPlatform.Api.Fleet.Interfaces.Rest.Transform.LocalizedOpenApiAssembler;
using FuelOpenApi = FlotixPlatform.Api.Fuel.Interfaces.Rest.Transform.LocalizedOpenApiAssembler;
using IncidentsOpenApi = FlotixPlatform.Api.Incidents.Interfaces.Rest.Transform.LocalizedOpenApiAssembler;
using MaintenanceOpenApi = FlotixPlatform.Api.Maintenance.Interfaces.Rest.Transform.LocalizedOpenApiAssembler;
using MonitoringOpenApi = FlotixPlatform.Api.Monitoring.Interfaces.Rest.Transform.LocalizedOpenApiAssembler;
using NotificationsOpenApi = FlotixPlatform.Api.Notifications.Interfaces.Rest.Transform.LocalizedOpenApiAssembler;

namespace FlotixPlatform.Api.Shared.Infrastructure.Interfaces.Asp.Configuration.Extensions;

/// <summary>
///     Dependency injection and web configuration of every bounded context of the Flotix API.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    ///     Registers repositories, command/query services and context facades of every bounded context.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <returns>The same service collection.</returns>
    public static IServiceCollection AddFlotixBoundedContexts(this IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        // ── Profiles
        services.AddScoped<FlotixPlatform.Api.Profiles.Domain.Repositories.IProfileRepository, FlotixPlatform.Api.Profiles.Infrastructure.Persistence.EntityFrameworkCore.Repositories.ProfileRepository>();
        services.AddScoped<FlotixPlatform.Api.Profiles.Application.CommandServices.IProfileCommandService, FlotixPlatform.Api.Profiles.Application.Internal.CommandServices.ProfileCommandService>();
        services.AddScoped<FlotixPlatform.Api.Profiles.Application.QueryServices.IProfileQueryService, FlotixPlatform.Api.Profiles.Application.Internal.QueryServices.ProfileQueryService>();
        services.AddScoped<FlotixPlatform.Api.Profiles.Interfaces.Acl.IProfilesContextFacade, FlotixPlatform.Api.Profiles.Application.Acl.ProfilesContextFacade>();

        // ── Fleet
        services.AddScoped<FlotixPlatform.Api.Fleet.Domain.Repositories.IVehicleRepository, FlotixPlatform.Api.Fleet.Infrastructure.Persistence.EntityFrameworkCore.Repositories.VehicleRepository>();
        services.AddScoped<FlotixPlatform.Api.Fleet.Application.Services.IVehicleCommandService, FlotixPlatform.Api.Fleet.Application.Internal.CommandServices.VehicleCommandService>();
        services.AddScoped<FlotixPlatform.Api.Fleet.Application.Services.IVehicleQueryService, FlotixPlatform.Api.Fleet.Application.Internal.QueryServices.VehicleQueryService>();
        services.AddScoped<FlotixPlatform.Api.Fleet.Interfaces.Acl.IFleetContextFacade, FlotixPlatform.Api.Fleet.Application.Acl.FleetContextFacade>();

        // ── Fuel
        services.AddScoped<FlotixPlatform.Api.Fuel.Domain.Repositories.IFuelRecordRepository, FlotixPlatform.Api.Fuel.Infrastructure.Persistence.EntityFrameworkCore.Repositories.FuelRecordRepository>();
        services.AddScoped<FlotixPlatform.Api.Fuel.Application.Services.IFuelRecordCommandService, FlotixPlatform.Api.Fuel.Application.Internal.CommandServices.FuelRecordCommandService>();
        services.AddScoped<FlotixPlatform.Api.Fuel.Application.Services.IFuelRecordQueryService, FlotixPlatform.Api.Fuel.Application.Internal.QueryServices.FuelRecordQueryService>();
        services.AddScoped<FlotixPlatform.Api.Fuel.Interfaces.ACL.IFuelContextFacade, FlotixPlatform.Api.Fuel.Application.ACL.FuelContextFacade>();

        // ── Maintenance
        services.AddScoped<FlotixPlatform.Api.Maintenance.Domain.Repositories.IMaintenanceRequestRepository, FlotixPlatform.Api.Maintenance.Infrastructure.Persistence.EntityFrameworkCore.Repositories.MaintenanceRequestRepository>();
        services.AddScoped<FlotixPlatform.Api.Maintenance.Application.Services.IMaintenanceRequestCommandService, FlotixPlatform.Api.Maintenance.Application.Internal.CommandServices.MaintenanceRequestCommandService>();
        services.AddScoped<FlotixPlatform.Api.Maintenance.Application.Services.IMaintenanceRequestQueryService, FlotixPlatform.Api.Maintenance.Application.Internal.QueryServices.MaintenanceRequestQueryService>();
        services.AddScoped<FlotixPlatform.Api.Maintenance.Interfaces.Acl.IMaintenanceContextFacade, FlotixPlatform.Api.Maintenance.Application.Acl.MaintenanceContextFacade>();

        // ── Incidents
        services.AddScoped<FlotixPlatform.Api.Incidents.Domain.Repositories.IIncidentRepository, FlotixPlatform.Api.Incidents.Infrastructure.Persistence.EntityFrameworkCore.Repositories.IncidentRepository>();
        services.AddScoped<FlotixPlatform.Api.Incidents.Application.Services.IIncidentCommandService, FlotixPlatform.Api.Incidents.Application.Internal.CommandServices.IncidentCommandService>();
        services.AddScoped<FlotixPlatform.Api.Incidents.Application.Services.IIncidentQueryService, FlotixPlatform.Api.Incidents.Application.Internal.QueryServices.IncidentQueryService>();

        // ── Monitoring
        services.AddScoped<FlotixPlatform.Api.Monitoring.Domain.Repositories.IIotDeviceRepository, FlotixPlatform.Api.Monitoring.Infrastructure.Persistence.EntityFrameworkCore.Repositories.IotDeviceRepository>();
        services.AddScoped<FlotixPlatform.Api.Monitoring.Domain.Repositories.ITelemetryDataRepository, FlotixPlatform.Api.Monitoring.Infrastructure.Persistence.EntityFrameworkCore.Repositories.TelemetryDataRepository>();
        services.AddScoped<FlotixPlatform.Api.Monitoring.Application.Services.IIotDeviceCommandService, FlotixPlatform.Api.Monitoring.Application.Internal.CommandServices.IotDeviceCommandService>();
        services.AddScoped<FlotixPlatform.Api.Monitoring.Application.Services.IIotDeviceQueryService, FlotixPlatform.Api.Monitoring.Application.Internal.QueryServices.IotDeviceQueryService>();
        services.AddScoped<FlotixPlatform.Api.Monitoring.Application.Services.ITelemetryDataCommandService, FlotixPlatform.Api.Monitoring.Application.Internal.CommandServices.TelemetryDataCommandService>();
        services.AddScoped<FlotixPlatform.Api.Monitoring.Application.Services.ITelemetryDataQueryService, FlotixPlatform.Api.Monitoring.Application.Internal.QueryServices.TelemetryDataQueryService>();

        // ── Notifications
        services.AddScoped<FlotixPlatform.Api.Notifications.Domain.Repositories.IAlertRepository, FlotixPlatform.Api.Notifications.Infrastructure.Persistence.EntityFrameworkCore.Repositories.AlertRepository>();
        services.AddScoped<FlotixPlatform.Api.Notifications.Domain.Repositories.INotificationRepository, FlotixPlatform.Api.Notifications.Infrastructure.Persistence.EntityFrameworkCore.Repositories.NotificationRepository>();
        services.AddScoped<FlotixPlatform.Api.Notifications.Application.Services.IAlertCommandService, FlotixPlatform.Api.Notifications.Application.Internal.CommandServices.AlertCommandService>();
        services.AddScoped<FlotixPlatform.Api.Notifications.Application.Services.INotificationCommandService, FlotixPlatform.Api.Notifications.Application.Internal.CommandServices.NotificationCommandService>();
        services.AddScoped<FlotixPlatform.Api.Notifications.Application.Services.IAlertQueryService, FlotixPlatform.Api.Notifications.Application.Internal.QueryServices.AlertQueryService>();
        services.AddScoped<FlotixPlatform.Api.Notifications.Application.Services.INotificationQueryService, FlotixPlatform.Api.Notifications.Application.Internal.QueryServices.NotificationQueryService>();

        // ── Commerce
        services.AddScoped<FlotixPlatform.Api.Commerce.Domain.Repositories.IIotOrderRepository, FlotixPlatform.Api.Commerce.Infrastructure.Persistence.EntityFrameworkCore.Repositories.IotOrderRepository>();
        services.AddScoped<FlotixPlatform.Api.Commerce.Application.Services.IIotOrderCommandService, FlotixPlatform.Api.Commerce.Application.Internal.CommandServices.IotOrderCommandService>();
        services.AddScoped<FlotixPlatform.Api.Commerce.Application.Services.IIotOrderQueryService, FlotixPlatform.Api.Commerce.Application.Internal.QueryServices.IotOrderQueryService>();

        // ── Analytics
        services.AddScoped<FlotixPlatform.Api.Analytics.Domain.Repositories.IReportRepository, FlotixPlatform.Api.Analytics.Infrastructure.Persistence.EntityFrameworkCore.Repositories.ReportRepository>();
        services.AddScoped<FlotixPlatform.Api.Analytics.Application.Services.IReportCommandService, FlotixPlatform.Api.Analytics.Application.Internal.CommandServices.ReportCommandService>();
        services.AddScoped<FlotixPlatform.Api.Analytics.Application.Services.IReportQueryService, FlotixPlatform.Api.Analytics.Application.Internal.QueryServices.ReportQueryService>();

        // ── Monitoring: telemetry queue, background processor and SignalR notifier
        services.AddSingleton<ITelemetryDataQueue, TelemetryDataQueue>();
        services.AddSingleton<IVehicleLocationNotifier, VehicleLocationNotifier>();
        services.AddHostedService<TelemetryDataProcessor>();

        return services;
    }

    /// <summary>
    ///     Registers the IAM bounded context: user repository, command/query services,
    ///     hashing/token services and the localized problem details factory.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="configuration">The application configuration.</param>
    /// <returns>The same service collection.</returns>
    public static IServiceCollection AddFlotixIam(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IUserCommandService, UserCommandService>();
        services.AddScoped<IUserQueryService, UserQueryService>();
        services.AddScoped<IIamContextFacade, IamContextFacade>();
        services.AddScoped<IHashingService, HashingService>();
        services.AddSingleton<ITokenService, TokenService>();
        services.AddScoped<ProblemDetailsFactory>();
        services.Configure<TokenSettings>(configuration.GetSection("TokenSettings"));
        
        // Demo data generated for every new account (DemoData:SeedOnSignUp, DemoData:HistoryDays)
        services.Configure<DemoDataSettings>(configuration.GetSection("DemoData"));
        services.AddScoped<IDemoDataSeeder, DemoDataSeeder>();

        return services;
    }

    /// <summary>
    ///     Registers the controllers (kebab-case routes, string enums) and the localized OpenAPI document.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <returns>The same service collection.</returns>
    public static IServiceCollection AddFlotixWebApi(this IServiceCollection services)
    {
        services.AddControllers(options => options.Conventions.Add(new KebabCaseRouteNamingConvention()))
            .AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));
        services.AddSignalR();
        services.AddProblemDetails();
        services.AddLocalization();
        services.AddExceptionHandler<FlotixPlatform.Api.Shared.Infrastructure.Pipeline.Middleware.Components.GlobalExceptionHandler>();

        services.AddOpenApi(options =>
        {
            options.AddDocumentTransformer<BearerSecuritySchemeTransformer>();
            options.AddOperationTransformer<IamBearerSecurityOperationTransformer>();
            options.AddOperationTransformer(FleetOpenApi.TransformOperationAsync);
            options.AddOperationTransformer(FuelOpenApi.TransformOperationAsync);
            options.AddOperationTransformer(MaintenanceOpenApi.TransformOperationAsync);
            options.AddOperationTransformer(IncidentsOpenApi.TransformOperationAsync);
            options.AddOperationTransformer(MonitoringOpenApi.TransformOperationAsync);
            options.AddOperationTransformer(NotificationsOpenApi.TransformOperationAsync);
            options.AddOperationTransformer(CommerceOpenApi.TransformOperationAsync);
            options.AddOperationTransformer(AnalyticsOpenApi.TransformOperationAsync);

            options.AddSchemaTransformer(FleetOpenApi.TransformSchemaAsync);
            options.AddSchemaTransformer(FuelOpenApi.TransformSchemaAsync);
            options.AddSchemaTransformer(MaintenanceOpenApi.TransformSchemaAsync);
            options.AddSchemaTransformer(IncidentsOpenApi.TransformSchemaAsync);
            options.AddSchemaTransformer(MonitoringOpenApi.TransformSchemaAsync);
            options.AddSchemaTransformer(NotificationsOpenApi.TransformSchemaAsync);
            options.AddSchemaTransformer(CommerceOpenApi.TransformSchemaAsync);
            options.AddSchemaTransformer(AnalyticsOpenApi.TransformSchemaAsync);
        });

        return services;
    }
}
