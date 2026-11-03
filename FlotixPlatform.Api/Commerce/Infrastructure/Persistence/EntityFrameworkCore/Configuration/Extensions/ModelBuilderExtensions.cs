using FlotixPlatform.API.IAM.Domain.Model.Aggregates;
using FlotixPlatform.API.Commerce.Domain.Model.Aggregates;
using FlotixPlatform.API.Commerce.Domain.Model.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace FlotixPlatform.API.Commerce.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;

/// <summary>
///     Entity Framework Core model configuration of the Digital Experience / IoT Commerce bounded context.
/// </summary>
/// <remarks>
///     The tables live in the <c>commerce</c> schema and reference other bounded contexts through
///     foreign keys with restricted deletes, as defined in the database design of the project report.
/// </remarks>
public static class ModelBuilderExtensions
{
    /// <summary>
    ///     Applies the entity configuration of the Digital Experience / IoT Commerce bounded context.
    /// </summary>
    /// <param name="builder">The model builder of the application database context.</param>
    public static void ApplyCommerceConfiguration(this ModelBuilder builder)
    {
        builder.Entity<IotOrder>().ToTable(nameof(IotOrder), "commerce");
        builder.Entity<IotOrder>().HasKey(i => i.Id);
        builder.Entity<IotOrder>().Property(i => i.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Entity<IotOrder>().Property(i => i.OwnerId).IsRequired();
        builder.Entity<IotOrder>().Property(i => i.Quantity).IsRequired();
        builder.Entity<IotOrder>().Property(i => i.TotalAmount).IsRequired();
        builder.Entity<IotOrder>().Property(i => i.Status).HasConversion<string>().HasMaxLength(50).IsRequired();
        builder.Entity<IotOrder>().Property(i => i.OrderDate).IsRequired();
        builder.Entity<IotOrder>()
            .HasOne<User>()
            .WithMany()
            .HasForeignKey(i => i.OwnerId)
            .OnDelete(DeleteBehavior.Restrict);
        builder.Entity<IotOrder>().HasIndex(i => i.OwnerId);
    }
}
