using FlotixPlatform.Api.Fleet.Domain.Model.Aggregates;
using FlotixPlatform.Api.Incidents.Domain.Model.Aggregates;
using FlotixPlatform.Api.Incidents.Domain.Model.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace FlotixPlatform.Api.Incidents.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;

/// <summary>
///     Entity Framework Core model configuration of the Incident Management bounded context.
/// </summary>
/// <remarks>
///     The tables live in the <c>incidents</c> schema and reference other bounded contexts through
///     foreign keys with restricted deletes, as defined in the database design of the project report.
/// </remarks>
public static class ModelBuilderExtensions
{
    /// <summary>
    ///     Applies the entity configuration of the Incident Management bounded context.
    /// </summary>
    /// <param name="builder">The model builder of the application database context.</param>
    public static void ApplyIncidentsConfiguration(this ModelBuilder builder)
    {
        builder.Entity<Incident>().ToTable(nameof(Incident), "incidents");
        builder.Entity<Incident>().HasKey(i => i.Id);
        builder.Entity<Incident>().Property(i => i.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Entity<Incident>().Property(i => i.VehicleId).IsRequired();
        builder.Entity<Incident>().Property(i => i.DriverId).IsRequired();
        builder.Entity<Incident>().Property(i => i.Description).IsRequired();
        builder.Entity<Incident>().Property(i => i.Status).HasConversion<string>().HasMaxLength(50).IsRequired();
        builder.Entity<Incident>().Property(i => i.ReportedDate).IsRequired();
        builder.Entity<Incident>()
            .HasOne<Vehicle>()
            .WithMany()
            .HasForeignKey(i => i.VehicleId)
            .OnDelete(DeleteBehavior.Restrict);
        builder.Entity<Incident>().HasIndex(i => i.VehicleId);
        builder.Entity<Incident>().HasIndex(i => i.DriverId);
    }
}
