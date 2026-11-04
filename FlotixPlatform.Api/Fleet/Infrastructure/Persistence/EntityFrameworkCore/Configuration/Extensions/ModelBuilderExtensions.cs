using FlotixPlatform.Api.Fleet.Domain.Model.Aggregates;
using FlotixPlatform.Api.Fleet.Domain.Model.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace FlotixPlatform.Api.Fleet.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;

/// <summary>
///     Entity Framework Core model configuration of the Vehicle & Fleet Management bounded context.
/// </summary>
/// <remarks>
///     The tables live in the <c>fleet</c> schema and reference other bounded contexts through
///     foreign keys with restricted deletes, as defined in the database design of the project report.
/// </remarks>
public static class ModelBuilderExtensions
{
    /// <summary>
    ///     Applies the entity configuration of the Vehicle & Fleet Management bounded context.
    /// </summary>
    /// <param name="builder">The model builder of the application database context.</param>
    public static void ApplyFleetConfiguration(this ModelBuilder builder)
    {
        builder.Entity<Vehicle>().ToTable(nameof(Vehicle), "fleet");
        builder.Entity<Vehicle>().HasKey(v => v.Id);
        builder.Entity<Vehicle>().Property(v => v.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Entity<Vehicle>().Property(v => v.OwnerId).IsRequired();
        builder.Entity<Vehicle>()
            .Property(v => v.LicensePlate)
            .HasConversion(valueObject => valueObject.Value, value => new LicensePlate(value))
            .HasMaxLength(15)
            .IsRequired();
        builder.Entity<Vehicle>().Property(v => v.VehicleType).IsRequired().HasMaxLength(50);
        builder.Entity<Vehicle>().Property(v => v.CurrentMileage).IsRequired();
        builder.Entity<Vehicle>().Property(v => v.Status).HasConversion<string>().HasMaxLength(50).IsRequired();
        builder.Entity<Vehicle>()
            .HasOne<User>()
            .WithMany()
            .HasForeignKey(v => v.OwnerId)
            .OnDelete(DeleteBehavior.Restrict);
        builder.Entity<Vehicle>()
            .HasOne<User>()
            .WithMany()
            .HasForeignKey(v => v.DriverId)
            .OnDelete(DeleteBehavior.Restrict);
        builder.Entity<Vehicle>().HasIndex(v => v.OwnerId);
        builder.Entity<Vehicle>().HasIndex(v => v.DriverId);
        builder.Entity<Vehicle>().HasIndex(v => v.LicensePlate).IsUnique();
    }
}