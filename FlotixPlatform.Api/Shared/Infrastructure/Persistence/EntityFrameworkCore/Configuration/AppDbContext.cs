using FlotixPlatform.Api.Analytics.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;
using FlotixPlatform.Api.Commerce.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;
using FlotixPlatform.Api.Fleet.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;
using FlotixPlatform.Api.Fuel.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;
using FlotixPlatform.Api.Iam.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;
using FlotixPlatform.Api.Incidents.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;
using FlotixPlatform.Api.Maintenance.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;
using FlotixPlatform.Api.Monitoring.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;
using FlotixPlatform.Api.Notifications.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;
using FlotixPlatform.Api.Profiles.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;
using FlotixPlatform.Api.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;
using FlotixPlatform.Api.Shared.Infrastructure.Persistence.EntityFrameworkCore.Interceptors;
using Microsoft.EntityFrameworkCore;

namespace FlotixPlatform.Api.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;

/// <summary>
///     Application database context for the Flotix Platform
/// </summary>
/// <param name="options">
///     The options for the database context
/// </param>
public class AppDbContext(DbContextOptions options) : DbContext(options)
{
    /// <inheritdoc />
    protected override void OnConfiguring(DbContextOptionsBuilder builder)
    {
        // Apply audit timestamp interceptor for all IAuditableEntity implementations
        builder.AddInterceptors(new AuditableEntityInterceptor());
        base.OnConfiguring(builder);
    }

    /// <summary>
    ///     On creating the database model
    /// </summary>
    /// <remarks>
    ///     This method is used to create the database model for the application.
    /// </remarks>
    /// <param name="builder">
    ///     The model builder for the database context
    /// </param>
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // IAM Context
        builder.ApplyIamConfiguration();

        // Profiles Context
        builder.ApplyProfilesConfiguration();

        // Fleet Context
        builder.ApplyFleetConfiguration();

        // Fuel Context
        builder.ApplyFuelConfiguration();

        // Maintenance Context
        builder.ApplyMaintenanceConfiguration();

        // Incidents Context
        builder.ApplyIncidentsConfiguration();

        // Monitoring Context
        builder.ApplyMonitoringConfiguration();

        // Notifications Context
        builder.ApplyNotificationsConfiguration();

        // Commerce Context
        builder.ApplyCommerceConfiguration();

        // Analytics Context
        builder.ApplyAnalyticsConfiguration();

        // General Naming Convention for the database objects
        builder.UseSnakeCaseNamingConvention();
    }
}
