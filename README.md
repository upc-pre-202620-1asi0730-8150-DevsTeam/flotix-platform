# Flotix Platform 

## Overview

Flotix Platform is an ASP.NET Core service that provides the REST API of Flotix, a fleet management solution for owners, drivers, and mechanics. The project is organized in the bounded contexts of the project report and follows simple command/query service patterns with Entity Framework Core persistence on PostgreSQL.

## Domain-Driven Design approach

The codebase applies a lightweight Domain-Driven Design style to keep each business capability easy to evolve:

- **Bounded contexts**
    - `Fleet`: owns the vehicle use cases (Vehicle & Fleet Management).
    - `Fuel`: owns the fuel record use cases (Fuel Control).
    - `Maintenance`: owns the maintenance request use cases (Maintenance Management).
    - `Incidents`: owns the incident use cases (Incident Management).
    - `Monitoring`: owns the IoT device and telemetry use cases (Real-Time Monitoring / Fleet Tracking).
    - `Notifications`: owns the alert and notification use cases (Alerts & Notifications).
    - `Commerce`: owns the IoT order use cases (Digital Experience / IoT Commerce).
    - `Analytics`: owns the report use cases (Reporting & Analytics).
    - `Shared`: owns cross-cutting abstractions used by multiple contexts, such as repository contracts, unit of work, auditing, naming conventions, and reusable application patterns.
- **Domain layer**
    - Contains the core business concepts: aggregates such as `Vehicle`, `FuelRecord`, `MaintenanceRequest`, and `TelemetryData`, value objects such as `LicensePlate`, `SerialNumber`, and `EmailAddress`, and command/query records.
    - Keeps business rules close to the model, including value-object validation and the maintenance request lifecycle.
- **Application layer**
    - Coordinates use cases through command/query services.
    - Returns explicit outcomes with a shared `Result<TValue, TError>` pattern for command handling.
    - Defines application-specific error types such as `CreateVehicleError`.
    - Reads other bounded contexts through context facades such as `IFleetContextFacade`.
- **Interfaces layer**
    - Exposes the REST API through controllers and ASP.NET Core OpenAPI metadata, and real-time events through SignalR hubs.
    - Uses assemblers to translate request resources into commands and application results into HTTP `ActionResult` responses.
- **Infrastructure layer**
    - Persists aggregates with Entity Framework Core and PostgreSQL, with one schema per bounded context and foreign keys with restricted deletes.
    - Maps value objects to database columns, applies migrations on startup, and manages audit timestamps through a save-changes interceptor.
    - Processes IoT telemetry in a background service.

This structure keeps HTTP concerns, persistence concerns, and business rules separated while still allowing the API to stay simple to consume.

## Features

- API base route: `/api/v1/{resource}` (kebab-case route naming convention), e.g. `/api/v1/vehicles`, `/api/v1/fuel-records`, `/api/v1/maintenance-requests`
- Create, list (with filters such as `?vehicleId=`), retrieve, update, and delete vehicles, fuel records, incidents, IoT devices, notifications, and IoT orders
- Receive IoT telemetry with `202 Accepted`, processed in the background and published as `VehicleLocationUpdated` on `/hubs/monitoring`
- Maintenance request flow: diagnostic and quote, budget decision, repair start, and completion
- Fuel consumption and maintenance cost reports by date range
- Alerts and per-user notifications in real time on `/hubs/notifications`
- Duplicate detection enforced at application and database level (returns `409 Conflict`)
- Localized validation responses (`400 Bad Request`)
- Unexpected errors returned as `ProblemDetails` (`500 Internal Server Error`)
- Entity Framework naming strategy for `snake_case` identifiers and plural table names
- Structured logging for diagnostics and observability

## Technologies

- C# 14 and .NET 10 (`net10.0`)
- ASP.NET Core Web API (REST controllers + built-in OpenAPI generation) and SignalR
- Scalar.AspNetCore for interactive API documentation
- Entity Framework Core 10 + PostgreSQL provider (`Npgsql.EntityFrameworkCore.PostgreSQL`)
- Humanizer for plural table names
- Localization with `.resx` resources (`en`, `en-US`, `es`, `es-PE`)
- Structured logging with `ILogger<T>`
- PlantUML (architecture diagrams in `docs/`)

## Technical stories

The API-focused technical stories for frontend integration are in [`docs/user-stories.md`](docs/user-stories.md).

## Class diagram

A PlantUML class diagram that reflects the code structure and bounded contexts is available at [`docs/class-diagram.puml`](docs/class-diagram.puml).

## Getting started (quick)

### Prerequisites

- .NET SDK 10
- Docker, or a reachable PostgreSQL instance

### 1) Restore dependencies

```bash
dotnet restore flotix-platform.sln
```

### 2) Configure connection string

Copy `.env.example` to `.env` and fill in the `DATABASE_*` values (see [Connecting to Neon](#connecting-to-neon-postgresql-and-keeping-secrets-out-of-git)). For a local PostgreSQL, use `DATABASE_HOST=localhost` and its credentials, and change `SSL Mode`/`Channel Binding` in your own `ConnectionStrings:DefaultConnection` user-secret.

### 3) Run the API

```bash
dotnet run --project FlotixPlatform.Api
```

Or from the API folder:

```bash
cd FlotixPlatform.Api
dotnet run
```

### Database initialization

#### Dotnet Tooling

Restore the EF Core tools and add a migration when the data model changes:
```bash
dotnet tool restore
cd FlotixPlatform.Api
dotnet ef migrations add <MigrationName>
dotnet ef database update
```

The application automatically applies pending EF Core migrations on startup via `Database.Migrate()`.

Note: The database must exist before running the application. On Neon it already exists; with a local PostgreSQL create it first.

- **Development**: Applies the current schema to the configured PostgreSQL database.
- **First-time setup**: Ensure the Neon database exists (the default `neondb` is created with the project) and the connection string points to it. Create the first migration with `dotnet ef migrations add InitialCreate` and commit it.
- **Schemas**: Each bounded context owns a schema (`fleet`, `fuel`, `maintenance`, `incidents`, `monitoring`, `notifications`, `commerce`, `analytics`).
- **Constraints**: Unique indexes are enforced on `license_plate`, `serial_number`, and the `vehicle_id` of IoT devices; foreign keys use `ON DELETE RESTRICT`.
- **Schema changes**: Add and commit EF Core migrations when the data model changes.

### 4) Explore the API with Scalar

With the included launch profiles, Scalar is commonly available at:

- `http://localhost:5080/scalar`
- `https://localhost:7080/scalar`

The generated OpenAPI document is available at `/openapi/v1.json`.

### Connecting to Neon (PostgreSQL) and keeping secrets out of Git

`appsettings.json`, `appsettings.Development.json` and `appsettings.Production.json` only contain `%PLACEHOLDERS%`, expanded at startup with `Environment.ExpandEnvironmentVariables`:

```json
"ConnectionStrings": {
  "DefaultConnection": "Host=%DATABASE_HOST%;Port=5432;Database=%DATABASE_NAME%;Username=%DATABASE_USER%;Password=%DATABASE_PASSWORD%;SSL Mode=Require;Channel Binding=Require;Timeout=30",
  "MigrationsConnection": "Host=%DATABASE_HOST_DIRECT%;Port=5432;..."
}
```

| Variable | Description |
|---|---|
| `DATABASE_HOST` | Neon **pooled** host (contains `-pooler`), used at runtime |
| `DATABASE_NAME` | Database name (e.g. `neondb`) |
| `DATABASE_USER` | Role (e.g. `neondb_owner`) |
| `DATABASE_PASSWORD` | Role password (**secret**) |
| `DATABASE_HOST_DIRECT` | Optional. Neon **direct** host (same host without `-pooler`), used only to apply migrations |
| `DATABASE_URL` | Optional alternative: the full Neon URL (`postgresql://...`), takes precedence over the variables above |
| `Cors__Origins__0`, ... | Allowed frontend origins |

**Local development:** copy `.env.example` to `.env` (git-ignored, loaded automatically in Development) and fill in the values. Another option is `dotnet user-secrets set "ConnectionStrings:DefaultConnection" "<connection string>"` from the `FlotixPlatform.Api` folder.

**Production (Render, Railway, Azure, Docker):** define the same variables as environment variables in the platform. Nothing secret is baked into the image (`.env` is excluded by `.dockerignore`).

Check the connection with `GET /health/db`. Set `Database__MigrateOnStartup=false` to skip migrations on startup.

Never put real values in `appsettings*.json`, `.env.example` or `launchSettings.json`. `.env`, `secrets.json`, keys and certificates are in `.gitignore`. Any future API key (payments, maps, email, etc.) must follow the same rule: read it from configuration and provide it through an environment variable or user-secrets.

## Error responses

The API returns standardized error responses:

- **400 Bad Request**: Request validation failed or a business rule rejected the data, and returns a localized error message.
- **404 Not Found**: Resource not found, or a referenced resource does not exist.
- **409 Conflict**: Duplicate values, invalid status transitions, or deletion of a resource that is still referenced.
- **500 Internal Server Error**: Unexpected server error (RFC7807 `ProblemDetails` format).

## Build and verify

```bash
dotnet build flotix-platform.sln
```