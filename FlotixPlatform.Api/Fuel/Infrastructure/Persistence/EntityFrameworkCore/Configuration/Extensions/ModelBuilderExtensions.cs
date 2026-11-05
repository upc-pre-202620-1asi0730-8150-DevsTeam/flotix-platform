using FlotixPlatform.Api.Fleet.Domain.Model.Aggregates;
using FlotixPlatform.Api.Fuel.Domain.Model.Aggregates;
using Microsoft.EntityFrameworkCore;

namespace FlotixPlatform.Api.Fuel.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;

/// <summary>
///     Entity Framework Core model configuration of the Fuel Control bounded context.
/// </summary>
/// <remarks>
///     The tables live in the <c>fuel</c> schema and reference other bounded contexts through
///     foreign keys with restricted deletes, as defined in the database design of the project report.
/// </remarks>
public static class ModelBuilderExtensions
{
    /// <summary>
    ///     Applies the entity configuration of the Fuel Control bounded context.
    /// </summary>
    /// <param name="builder">The model builder of the application database context.</param>
    public static void ApplyFuelConfiguration(this ModelBuilder builder)
    {
        builder.Entity<FuelRecord>().ToTable(nameof(FuelRecord), "fuel");
        builder.Entity<FuelRecord>().HasKey(f => f.Id);
        builder.Entity<FuelRecord>().Property(f => f.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Entity<FuelRecord>().Property(f => f.VehicleId).IsRequired();
        builder.Entity<FuelRecord>().Property(f => f.DriverId).IsRequired();
        builder.Entity<FuelRecord>().Property(f => f.Liters).IsRequired();
        builder.Entity<FuelRecord>().Property(f => f.Cost).IsRequired();
        builder.Entity<FuelRecord>().Property(f => f.MileageAtRefill).IsRequired();
        builder.Entity<FuelRecord>().Property(f => f.RecordedDate).IsRequired();
        builder.Entity<FuelRecord>()
            .HasOne<Vehicle>()
            .WithMany()
            .HasForeignKey(f => f.VehicleId)
            .OnDelete(DeleteBehavior.Restrict);
        builder.Entity<FuelRecord>().HasIndex(f => f.VehicleId);
        builder.Entity<FuelRecord>().HasIndex(f => f.DriverId);
    }
}
