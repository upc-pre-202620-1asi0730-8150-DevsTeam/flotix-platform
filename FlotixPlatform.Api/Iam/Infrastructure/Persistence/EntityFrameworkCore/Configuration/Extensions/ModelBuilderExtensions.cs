using FlotixPlatform.Api.Iam.Domain.Model.Aggregates;
using FlotixPlatform.Api.Iam.Domain.Model.ValueObjects;
using FlotixPlatform.Api.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration.ValueConverters;
using Microsoft.EntityFrameworkCore;

namespace FlotixPlatform.Api.Iam.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;

public static class ModelBuilderExtensions
{
    public static void ApplyIamConfiguration(this ModelBuilder builder)
    {
        // IAM Context

        builder.Entity<User>().HasKey(u => u.Id);
        builder.Entity<User>().Property(u => u.Id)
            .HasConversion(new StronglyTypedIdConverter<UserId, int>(id => id.Value, value => new UserId(value)))
            .IsRequired()
            .ValueGeneratedOnAdd();
        builder.Entity<User>().Property(u => u.Username).IsRequired();
        builder.Entity<User>().HasIndex(u => u.Username).IsUnique();
        builder.Entity<User>().Property(u => u.PasswordHash).IsRequired();
    }
}