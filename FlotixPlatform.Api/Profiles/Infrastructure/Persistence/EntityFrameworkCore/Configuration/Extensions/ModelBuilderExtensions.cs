using FlotixPlatform.Api.Profiles.Domain.Model.Aggregates;
using FlotixPlatform.Api.Profiles.Domain.Model.ValueObjects;
using FlotixPlatform.Api.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration.ValueConverters;
using Microsoft.EntityFrameworkCore;

namespace FlotixPlatform.Api.Profiles.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;

public static class ModelBuilderExtensions
{
    public static void ApplyProfilesConfiguration(this ModelBuilder builder)
    {
        // Profiles Context

        builder.Entity<Profile>().HasKey(p => p.Id);
        builder.Entity<Profile>().Property(p => p.Id)
            .HasConversion(new StronglyTypedIdConverter<ProfileId, int>(id => id.Value, value => new ProfileId(value)))
            .IsRequired()
            .ValueGeneratedOnAdd();
        builder.Entity<Profile>().OwnsOne(p => p.Name,
            n =>
            {
                n.WithOwner().HasForeignKey("Id");
                n.Property(p => p.FirstName).HasColumnName("FirstName").IsRequired().HasMaxLength(50);
                n.Property(p => p.LastName).HasColumnName("LastName").IsRequired().HasMaxLength(50);
            });

        builder.Entity<Profile>().OwnsOne(p => p.Email,
            e =>
            {
                e.WithOwner().HasForeignKey("Id");
                e.Property(a => a.Address).HasColumnName("EmailAddress").IsRequired().HasMaxLength(100);
                e.HasIndex(a => a.Address).IsUnique();
            });

        builder.Entity<Profile>().Property(p => p.Role).HasConversion<string>().IsRequired().HasMaxLength(20);
    }
}
