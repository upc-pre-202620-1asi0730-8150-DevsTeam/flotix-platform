using FlotixPlatform.Api.Monitoring.Interfaces.Hubs;
using FlotixPlatform.Api.Notifications.Interfaces.Hubs;
using FlotixPlatform.Api.Shared.Infrastructure.Interfaces.Asp.Configuration.Extensions;
using FlotixPlatform.Api.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

// In development, load the git-ignored ".env" file (project folder or repository root) into the environment.
if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development")
{
    DotEnvLoader.Load(Directory.GetCurrentDirectory());
    DotEnvLoader.Load(Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "..")));
}

var builder = WebApplication.CreateBuilder(args);

// ── Database (Neon PostgreSQL) ────────────────────────────────────────────────
// appsettings*.json only hold %PLACEHOLDERS% (e.g. %DATABASE_PASSWORD%) that are expanded from environment
// variables or the git-ignored .env file, so no secret is ever committed. See README.
var connectionString = builder.Configuration.GetDatabaseConnectionString();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString, npgsql => npgsql.EnableRetryOnFailure(3)));

// ── Bounded contexts, controllers, OpenAPI and localization ───────────────────
builder.Services.AddFlotixBoundedContexts();
builder.Services.AddFlotixWebApi();

// ── CORS (allowed frontend origins come from configuration: Cors:Origins) ─────
var allowedOrigins = builder.Configuration.GetSection("Cors:Origins").Get<string[]>() ?? [];
builder.Services.AddCors(options =>
    options.AddPolicy("AllowFrontend", policy =>
    {
        if (allowedOrigins.Length > 0)
            policy.WithOrigins(allowedOrigins).AllowAnyHeader().AllowAnyMethod().AllowCredentials();
        else
            policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
    }));

var app = builder.Build();

// ── Migrations on startup ─────────────────────────────────────────────────────
// Neon's pooled host (-pooler) runs PgBouncer, which does not support the advisory locks EF Core
// uses to apply migrations, so the direct endpoint is used for this step when it is configured.
if (app.Configuration.GetValue("Database:MigrateOnStartup", true))
{
    var migrationsConnectionString = app.Configuration.GetMigrationsConnectionString();
    if (migrationsConnectionString is null)
    {
        using var scope = app.Services.CreateScope();
        scope.ServiceProvider.GetRequiredService<AppDbContext>().Database.Migrate();
    }
    else
    {
        var migrationsOptions = new DbContextOptionsBuilder<AppDbContext>()
            .UseNpgsql(migrationsConnectionString)
            .Options;
        using var migrationsContext = new AppDbContext(migrationsOptions);
        migrationsContext.Database.Migrate();
    }
}

// ── Middleware ────────────────────────────────────────────────────────────────
app.UseExceptionHandler();

if (app.Environment.IsDevelopment()) app.UseHttpsRedirection();

app.UseRequestLocalization(new RequestLocalizationOptions()
    .SetDefaultCulture("en")
    .AddSupportedCultures("en", "en-US", "es", "es-PE")
    .AddSupportedUICultures("en", "en-US", "es", "es-PE"));

app.UseCors("AllowFrontend");

app.MapOpenApi();
app.MapScalarApiReference();

app.MapControllers();
app.MapHub<MonitoringHub>("/hubs/monitoring");
app.MapHub<NotificationsHub>("/hubs/notifications");

// Reports whether the API can reach the database (useful to verify the Neon connection).
app.MapGet("/health/db", async (AppDbContext context, CancellationToken cancellationToken) =>
    await context.Database.CanConnectAsync(cancellationToken)
        ? Results.Ok(new { status = "Healthy" })
        : Results.Problem("The database is not reachable.", statusCode: StatusCodes.Status503ServiceUnavailable))
    .ExcludeFromDescription();

app.Run();
