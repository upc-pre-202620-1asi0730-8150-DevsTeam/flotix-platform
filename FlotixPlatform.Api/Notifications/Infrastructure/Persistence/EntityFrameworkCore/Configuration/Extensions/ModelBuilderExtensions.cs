using FlotixPlatform.Api.Fleet.Domain.Model.Aggregates;
using FlotixPlatform.Api.Notifications.Domain.Model.Aggregates;
using Microsoft.EntityFrameworkCore;

namespace FlotixPlatform.Api.Notifications.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;

/// <summary>
///     Entity Framework Core model configuration of the Alerts & Notifications bounded context.
/// </summary>
/// <remarks>
///     The tables live in the <c>notifications</c> schema and reference other bounded contexts through
///     foreign keys with restricted deletes, as defined in the database design of the project report.
/// </remarks>
public static class ModelBuilderExtensions
{
    /// <summary>
    ///     Applies the entity configuration of the Alerts & Notifications bounded context.
    /// </summary>
    /// <param name="builder">The model builder of the application database context.</param>
    public static void ApplyNotificationsConfiguration(this ModelBuilder builder)
    {
        builder.Entity<Alert>().ToTable(nameof(Alert), "notifications");
        builder.Entity<Alert>().HasKey(a => a.Id);
        builder.Entity<Alert>().Property(a => a.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Entity<Alert>().Property(a => a.VehicleId).IsRequired();
        builder.Entity<Alert>().Property(a => a.AlertType).IsRequired().HasMaxLength(50);
        builder.Entity<Alert>().Property(a => a.TriggerDate).IsRequired();
        builder.Entity<Alert>()
            .HasOne<Vehicle>()
            .WithMany()
            .HasForeignKey(a => a.VehicleId)
            .OnDelete(DeleteBehavior.Restrict);
        builder.Entity<Alert>().HasIndex(a => a.VehicleId);

        builder.Entity<Notification>().ToTable(nameof(Notification), "notifications");
        builder.Entity<Notification>().HasKey(n => n.Id);
        builder.Entity<Notification>().Property(n => n.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Entity<Notification>().Property(n => n.UserId).IsRequired();
        builder.Entity<Notification>().Property(n => n.AlertId);
        builder.Entity<Notification>().Property(n => n.Message).IsRequired();
        builder.Entity<Notification>().Property(n => n.IsRead).IsRequired();
        builder.Entity<Notification>().Property(n => n.SentDate).IsRequired();
        builder.Entity<Notification>()
            .HasOne<Alert>()
            .WithMany()
            .HasForeignKey(n => n.AlertId)
            .OnDelete(DeleteBehavior.Restrict);
        builder.Entity<Notification>().HasIndex(n => n.UserId);
        builder.Entity<Notification>().HasIndex(n => n.AlertId);
    }
}
