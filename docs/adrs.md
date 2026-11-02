# Architecture Decision Records (ADRs)

## About this document

This document records the significant architectural decisions of the Flotix Platform REST API. Each record follows the lightweight format proposed by Michael Nygard: **Context**, **Decision**, **Consequences**, plus a **Status** and **Date**.

Guidelines for maintaining this log:

- One decision per record; records are short and focused on the *why*.
- Records are immutable once **Accepted**. To change a decision, add a new ADR that **Supersedes** the old one and update the old one's status.
- Statuses: `Proposed`, `Accepted`, `Deprecated`, `Superseded by ADR-XXX`.
- Number records sequentially and never reuse numbers.

## Index

| ADR | Title | Status |
|-----|-------|--------|
| [ADR-001](#adr-001--organize-the-code-by-bounded-context-with-a-layered-ddd-structure) | Organize the code by bounded context with a layered DDD structure | Accepted |
| [ADR-002](#adr-002--separate-command-and-query-services-cqs) | Separate command and query services (CQS) | Accepted |
| [ADR-003](#adr-003--model-expected-failures-with-a-result-type) | Model expected failures with a `Result` type | Accepted |
| [ADR-004](#adr-004--use-value-objects-for-emailaddress-licenseplate-and-serialnumber) | Use value objects for `EmailAddress`, `LicensePlate` and `SerialNumber` | Accepted |
| [ADR-005](#adr-005--map-rest-resources-with-static-assemblers) | Map REST resources with static assemblers | Accepted |
| [ADR-006](#adr-006--persist-with-ef-core-on-postgresql-using-repository-and-unit-of-work) | Persist with EF Core on PostgreSQL using Repository and Unit of Work | Accepted |
| [ADR-007](#adr-007--use-snake_case-database-naming-and-automatic-audit-timestamps) | Use `snake_case` database naming and automatic audit timestamps | Accepted |
| [ADR-008](#adr-008--apply-database-migrations-on-startup) | Apply database migrations on startup | Accepted |
| [ADR-009](#adr-009--use-lowercase-kebab-case-routes) | Use lowercase kebab-case routes | Accepted |
| [ADR-010](#adr-010--report-errors-as-rfc-7807-problemdetails) | Report errors as RFC 7807 `ProblemDetails` | Accepted |
| [ADR-011](#adr-011--localize-user-facing-text-with-resx-resources) | Localize user-facing text with `.resx` resources | Accepted |
| [ADR-012](#adr-012--keep-the-jwt-signing-secret-out-of-source-control) | Keep the JWT signing secret out of source control | Accepted |
| [ADR-013](#adr-013--use-built-in-openapi-and-scalar-instead-of-swashbuckle) | Use built-in OpenAPI and Scalar instead of Swashbuckle | Accepted |
| [ADR-014](#adr-014--use-one-database-schema-per-bounded-context-and-foreign-keys-with-restricted-deletes) | Use one database schema per bounded context and foreign keys with restricted deletes | Accepted |
| [ADR-015](#adr-015--use-uuid-identifiers-and-the-column-names-of-the-report) | Use UUID identifiers and the column names of the report | Accepted |
| [ADR-016](#adr-016--store-users-with-table-per-hierarchy-inheritance) | Store users with table-per-hierarchy inheritance | Accepted |
| [ADR-017](#adr-017--use-the-spanish-status-values-of-the-report-on-the-wire) | Use the Spanish status values of the report on the wire | Accepted |
| [ADR-018](#adr-018--authorize-requests-with-role-based-jwt-through-an-iam-middleware-disconnected-by-default) | Authorize requests with role-based JWT through an IAM middleware, disconnected by default | Accepted |
| [ADR-019](#adr-019--process-iot-telemetry-in-the-background) | Process IoT telemetry in the background | Accepted |
| [ADR-020](#adr-020--push-real-time-events-with-signalr) | Push real-time events with SignalR | Accepted |
| [ADR-021](#adr-021--model-the-maintenance-lifecycle-with-explicit-commands) | Model the maintenance lifecycle with explicit commands | Accepted |
| [ADR-022](#adr-022--read-other-bounded-contexts-through-context-facades) | Read other bounded contexts through context facades | Accepted |

---

## ADR-001 — Organize the code by bounded context with a layered DDD structure

- **Status:** Accepted
- **Date:** 2026-10-28

### Context
The API covers the nine bounded contexts of the project report (Identity, Profiles & Security; Vehicle & Fleet Management; Fuel Control; Maintenance Management; Incident Management; Real-Time Monitoring / Fleet Tracking; Alerts & Notifications; Digital Experience / IoT Commerce; Reporting & Analytics). We need a structure that keeps domain rules independent from frameworks and prevents unrelated features from becoming entangled.

### Decision
Use a single deployable project organized first by bounded context (`IAM`, `Fleet`, `Fuel`, `Maintenance`, `Incidents`, `Monitoring`, `Notifications`, `Commerce`, `Analytics`, `Shared`) and then by layer:

- `Domain` — aggregates, value objects, commands, queries, repository contracts.
- `Application` — service contracts (`Services`), implementations (`Internal`), outbound services, context facades (`ACL`), and application errors.
- `Infrastructure` — persistence and other technical adapters.
- `Interfaces` — REST controllers, resources, assemblers, SignalR hubs, and context facade contracts (`ACL`).

Dependencies point inward: `Interfaces` and `Infrastructure` depend on `Application` and `Domain`, never the reverse. Cross-context technical concerns live in `Shared`.

### Consequences
- (+) Domain logic is testable without ASP.NET Core or EF Core.
- (+) Each bounded context evolves in its own sibling folder.
- (−) More folders and types than a simple CRUD layout.
- (−) The dependency rule is enforced by convention only; a future split into separate projects or architecture tests could enforce it.

---

## ADR-002 — Separate command and query services (CQS)

- **Status:** Accepted
- **Date:** 2026-10-28

### Context
Writes (registering a vehicle, submitting a maintenance request) have validation, uniqueness rules, and persistence side effects, while reads are simple lookups. Mixing both in one service hides these differences.

### Decision
Expose two application services per aggregate, such as `IVehicleCommandService` (handles commands such as `CreateVehicleCommand`) and `IVehicleQueryService` (handles queries such as `GetVehicleByIdQuery`). Commands and queries are immutable records; every handler accepts a `CancellationToken`.

### Consequences
- (+) Clear intent and smaller, focused classes.
- (+) Read and write paths can evolve independently (e.g., read models later).
- (−) Slightly more boilerplate per use case.
- This is CQS at the service level, not full CQRS: both sides share one database and model.

---

## ADR-003 — Model expected failures with a `Result` type

- **Status:** Accepted
- **Date:** 2026-10-28

### Context
Duplicate license plates, unknown references, invalid status transitions, and similar outcomes are expected business results, not exceptional conditions. Using exceptions for them obscures control flow and is costly.

### Decision
Command handlers return `Result<TValue, TError>` (`Shared.Application.Patterns`) with an error enum per command, such as `CreateVehicleError`. The REST layer translates the result into an HTTP response through assemblers such as `ActionResultFromCreateVehicleResultAssembler`. Exceptions are reserved for unexpected failures and are handled globally (see ADR-010).

### Consequences
- (+) Failure modes are explicit in the method signature and exhaustively mappable to HTTP status codes.
- (+) No exception-driven control flow for business rules.
- (−) Callers must handle both branches; a custom type is maintained instead of using a library.

---

## ADR-004 — Use value objects for `EmailAddress`, `LicensePlate` and `SerialNumber`

- **Status:** Accepted
- **Date:** 2026-10-28

### Context
Email addresses, license plates, and IoT serial numbers are strings with validation rules and uniqueness constraints. Passing raw strings around allows invalid values and parameter mix-ups.

### Decision
Model them as immutable value objects (`EmailAddress`, `LicensePlate`, `SerialNumber`) that validate and normalize on construction. EF Core maps them to string columns with value converters. Raw strings are only accepted at the API boundary.

### Consequences
- (+) Invalid values cannot enter the domain and duplicates are detected on normalized values.
- (−) Requires value converters and mapping code.

---

## ADR-005 — Map REST resources with static assemblers

- **Status:** Accepted
- **Date:** 2026-10-28

### Context
Domain objects must not be exposed directly through the API (e.g., the password hash), and controllers should stay thin.

### Decision
Use dedicated resources (`CreateVehicleResource`, `VehicleResource`) and static assembler classes in `Interfaces/Rest/Transform`, named `<Target>From<Source>Assembler` (e.g. `VehicleResourceFromEntityAssembler`). The localized OpenAPI metadata assembler of each bounded context (`LocalizedOpenApiAssembler`) follows the same placement.

### Consequences
- (+) The public contract is decoupled from the domain model; controllers contain no mapping logic.
- (+) Pure static functions are trivial to unit test.
- (−) No automatic mapping; each new resource needs an assembler. Assemblers needing services (e.g. localizers) receive them as parameters.

---

## ADR-006 — Persist with EF Core on PostgreSQL using Repository and Unit of Work

- **Status:** Accepted
- **Date:** 2026-10-28

### Context
The database design of the project report defines a relational PostgreSQL database with unique constraints (e.g. `license_plate`, `serial_number`, `email`) and the service needs a way to commit changes atomically per use case.

### Decision
Use Entity Framework Core with the PostgreSQL provider (`Npgsql.EntityFrameworkCore.PostgreSQL`) and a PostgreSQL instance in Docker (`docker-compose.yml`). Define repository interfaces in the domain (`IBaseRepository<T>`, `IVehicleRepository`), implement them in infrastructure, and commit through `IUnitOfWork.CompleteAsync`. Repositories do not save changes themselves. The connection string comes from configuration and supports environment variable expansion so secrets are not committed.

### Consequences
- (+) Domain and application code depend on abstractions, not EF Core.
- (+) A single commit point per use case.
- (+) Unique indexes back the duplicate rules at the database level.
- (−) Switching databases requires changing the provider and re-validating migrations.

---

## ADR-007 — Use `snake_case` database naming and automatic audit timestamps

- **Status:** Accepted
- **Date:** 2026-10-28

### Context
PostgreSQL conventions favor `snake_case`; C# favors `PascalCase`. Audit timestamps should not depend on each handler remembering to set them.

### Decision
Apply a custom `UseSnakeCaseNamingConvention` in `AppDbContext` for tables, columns, keys, and indexes. Entities implementing `IAuditableEntity` receive `CreatedAt`/`UpdatedAt` automatically via `AuditableEntityInterceptor` on `SavingChanges`/`SavingChangesAsync`. The `created_at` and `updated_at` columns are technical columns added to the database design of the report.

### Consequences
- (+) Idiomatic naming on both sides without per-property attributes.
- (+) Audit data is consistent and cannot be forgotten.
- (−) Custom convention code must be maintained; raw SQL must use the snake_case names.

---

## ADR-008 — Apply database migrations on startup

- **Status:** Accepted
- **Date:** 2026-10-28

### Context
The project should run with minimal manual setup in development and in containers.

### Decision
Call `Database.Migrate()` when the application starts. It is idempotent when the schema is up to date.

### Consequences
- (+) Zero-step local and demo deployments.
- (−) Not recommended for multi-instance production: concurrent startups can race, and the application needs schema-altering privileges.
- Revisit (new ADR) before production use: run migrations as a separate deployment step or a migration bundle.

---

## ADR-009 — Use lowercase kebab-case routes

- **Status:** Accepted
- **Date:** 2026-10-28

### Context
Consistent, predictable URLs are part of the API contract (`/api/v1/fuel-records`, `/api/v1/maintenance-requests`).

### Decision
Enable `LowercaseUrls` and register `KebabCaseRouteNamingConvention` so controller names such as `FuelRecords` become `fuel-records`. Controllers use `[Route("api/v1/[controller]")]`; versioning is expressed in the path (`v1`).

### Consequences
- (+) Uniform URLs without per-controller route boilerplate.
- (−) Renaming a controller changes its public URL; treat controller names as contract.

---

## ADR-010 — Report errors as RFC 7807 `ProblemDetails`

- **Status:** Accepted
- **Date:** 2026-10-28

### Context
Clients need a consistent, machine-readable error shape for validation errors and server failures, without leaking internals.

### Decision
Use `AddProblemDetails` with `UseExceptionHandler` for unexpected errors (HTTP 500), and a custom `InvalidModelStateResponseFactory` returning a validation `ProblemDetails` (`application/problem+json`, HTTP 400). Unexpected `Result` failures are mapped to `ProblemDetails` (HTTP 500) by the assemblers, while expected failures return `400 Bad Request`, `401 Unauthorized`, `404 Not Found`, or `409 Conflict` with a localized message. Titles and details are localized (ADR-011).

### Consequences
- (+) One error contract across the API, documented in the OpenAPI document via `[ProducesResponseType]`.
- (+) Exception details are not exposed to clients.
- (−) Handlers must not throw for expected conditions (see ADR-003).
- (−) Expected failures return a plain localized message rather than `ProblemDetails`; aligning them is a possible follow-up.

---

## ADR-011 — Localize user-facing text with `.resx` resources

- **Status:** Accepted
- **Date:** 2026-10-28

### Context
Clients in different languages consume the API, and error messages and API documentation should follow the caller's language.

### Decision
Use ASP.NET Core localization with a shared marker class (`SharedResource`) and `.resx` files for the neutral, `en`, `en-US`, `es`, and `es-PE` cultures; `en` is the default. `UseRequestLocalization` selects the culture from the request (e.g., `Accept-Language`) and runs before OpenAPI mapping so generated documentation follows it. Data annotation messages use keys (`[Required(ErrorMessage = "RequiredLicensePlate")]`) resolved from `SharedResource`. All resource files must keep the same set of keys.

Internal log messages and exception messages are intentionally **not** localized, to keep operational logs searchable and consistent.

### Consequences
- (+) Localized errors, validation messages, and API descriptions.
- (+) Adding a language is a new `.resx` file plus a supported-culture entry.
- (−) Every new user-facing string requires entries in all resource files.
- (−) The generated OpenAPI document varies by request culture, so caches must vary on `Accept-Language`.

---

## ADR-012 — Keep the JWT signing secret out of source control

- **Status:** Accepted
- **Date:** 2026-10-28

### Context
Tokens are signed with a symmetric secret. Committing it would let anyone with access to the repository forge tokens.

### Decision
Read the secret from `TokenSettings:Secret`. Only `appsettings.Development.json` holds a development secret; production provides it through configuration (e.g. the `TokenSettings__Secret` environment variable). `TokenSettings` is validated on startup and the application does not start when the secret is missing or shorter than 32 bytes.

### Consequences
- (+) Production secrets never reach the repository.
- (+) A misconfigured deployment fails fast instead of issuing weak tokens.
- (−) Every environment must provide the secret before the first start.

---

## ADR-013 — Use built-in OpenAPI and Scalar instead of Swashbuckle

- **Status:** Accepted
- **Date:** 2026-10-28

### Context
The API must be documented with OpenAPI (TS31 of the project report). ASP.NET Core ships built-in OpenAPI document generation (`Microsoft.AspNetCore.OpenApi`), and Microsoft documents it as the supported path, while the UI is an independent choice.

### Decision
Generate the document with `AddOpenApi`/`MapOpenApi` (served at `/openapi/v1.json`) and serve the API explorer with `Scalar.AspNetCore` (`/scalar`). Localized summaries, descriptions, and schema texts are applied through operation and schema transformers in the `LocalizedOpenApiAssembler` of each bounded context. Response metadata uses `[ProducesResponseType]`. Swashbuckle and Swagger UI are not used.

The document and UI are mapped in all environments.

### Consequences
- (+) Fewer third-party dependencies for document generation; first-party support and updates.
- (+) Modern UI with request samples.
- (−) Transformers must be registered per bounded context.
- (−) Exposing the explorer in production is a security and information-disclosure choice; restrict it to Development, or protect it, before a production release (new ADR).

---

## ADR-014 — Use one database schema per bounded context and foreign keys with restricted deletes

- **Status:** Accepted
- **Date:** 2026-10-28

### Context
The database design of the project report groups the tables by bounded context and relates them with foreign keys (e.g. `vehicles.owner_id → users.id`, `fuel_records.vehicle_id → vehicles.id`, `notifications.alert_id → alerts.id`). The report also justifies the history tables by traceability.

### Decision
Each bounded context owns a PostgreSQL schema (`iam`, `fleet`, `fuel`, `maintenance`, `incidents`, `monitoring`, `notifications`, `commerce`, `analytics`). The relations of the report are foreign keys with `ON DELETE RESTRICT`, configured without navigation properties (`HasOne<Vehicle>().WithMany().HasForeignKey(...)`), so aggregates still reference other contexts by identifier only. A foreign key violation (SQLSTATE `23503`) is mapped to `404 Not Found` when creating or updating and to `409 Conflict` when deleting a referenced row.

### Consequences
- (+) The schema matches the report and the database guarantees referential integrity.
- (+) Deleting a vehicle or user with history is rejected instead of losing records.
- (−) Contexts are coupled at the database level; extracting one requires removing its foreign keys.
- (−) Rows with dependents must be removed explicitly before deletion.

---

## ADR-015 — Use UUID identifiers and the column names of the report

- **Status:** Accepted
- **Date:** 2026-10-28

### Context
Every table of the database design of the project report uses a `UUID` primary key and defines its column names (`license_plate`, `current_mileage`, `mileage_at_refill`, `recorded_date`, ...).

### Decision
Aggregates use `Guid` identifiers generated on insert and properties named after the report columns, so the snake_case convention (ADR-007) produces the report names.

### Consequences
- (+) Identifiers are not guessable and the schema matches the report.
- (−) Larger keys than integers.

---

## ADR-016 — Store users with table-per-hierarchy inheritance

- **Status:** Accepted
- **Date:** 2026-10-28

### Context
The class diagram of the project report models `User` as an abstract class specialized by `Owner`, `Driver`, and `Mechanic`, and the database design stores them in a single `users` table with a `role_type` column.

### Decision
`User` is an abstract aggregate with the `Owner` (`CompanyName`), `Driver` (`LicenseNumber`, `LicenseExpiration`), and `Mechanic` (`WorkshopName`) subclasses, mapped with table-per-hierarchy inheritance and `role_type` as discriminator. The `User.Create` factory method builds the subclass for the requested role; a driver requires a license number and expiration.

### Consequences
- (+) One table as in the report; role-specific rules live in each subclass.
- (−) Role-specific columns are nullable at the database level.

---

## ADR-017 — Use the Spanish status values of the report on the wire

- **Status:** Accepted
- **Date:** 2026-10-28

### Context
The project report and the frontend use Spanish status values (e.g. `En mantenimiento`, `Pendiente de pago`, `Reporte de consumo de combustible`).

### Decision
Enums are English in code, persisted by member name, and serialized with `JsonStringEnumMemberName` using the Spanish values.

### Consequences
- (+) The API speaks the language of the report and the frontend.
- (−) Renaming a wire value is a breaking API change.

---

## ADR-018 — Authorize requests with role-based JWT through an IAM middleware, disconnected by default

- **Status:** Accepted
- **Date:** 2026-10-28

### Context
The Auth Service of the project report issues JSON Web Tokens with role-based authorization for owners, drivers, and mechanics, while the frontend still runs against open endpoints.

### Decision
Hash passwords with the ASP.NET Core Identity password hasher (`HashingService`) and issue HS256 JWTs with the user id and role through `TokenService` (`Microsoft.AspNetCore.Authentication.JwtBearer`). `RequestAuthorizationMiddleware` enforces `[AllowAnonymous]` and `[Authorize(Role...)]` and is registered but commented out in `Program.cs` (`// app.UseRequestAuthorization();`) until the frontend sends tokens.

### Consequences
- (+) Stateless tokens as described in the report; enabling protection is a one-line change.
- (−) While disconnected, every endpoint is public.

---

## ADR-019 — Process IoT telemetry in the background

- **Status:** Accepted
- **Date:** 2026-10-28

### Context
Telemetry packets must be stored, update the vehicle location in real time, and be discarded with an error log when corrupt (TS02 of the project report). The C4 component diagram of Real-Time Monitoring includes an IoT Telemetry Processor.

### Decision
`POST /api/v1/telemetry-data` validates the packet and enqueues it in a bounded `System.Threading.Channels` queue (`ITelemetryDataQueue`), responding `202 Accepted`. `TelemetryDataProcessor`, a `BackgroundService`, dequeues each packet and processes it through `ITelemetryDataCommandService`: packets of unlinked devices are discarded with an error log; otherwise the reading is stored, the device is marked as connected, and the location is published (ADR-020).

### Consequences
- (+) Devices get a fast response and ingestion is decoupled from persistence.
- (−) Queued packets are lost if the process stops; a durable broker can replace the queue behind `ITelemetryDataQueue` (new ADR).

---

## ADR-020 — Push real-time events with SignalR

- **Status:** Accepted
- **Date:** 2026-10-28

### Context
Vehicle locations, alerts, and notifications must reach dashboards without polling.

### Decision
Use ASP.NET Core SignalR with strongly typed hubs per bounded context: `MonitoringHub` (`/hubs/monitoring`) sends `VehicleLocationUpdated`, and `NotificationsHub` (`/hubs/notifications`) sends `AlertActivated` and `NotificationReceived`, the latter only to its recipient.

### Consequences
- (+) Built into ASP.NET Core, no extra dependency.
- (−) Per-user delivery requires the IAM middleware to identify connections.

---

## ADR-021 — Model the maintenance lifecycle with explicit commands

- **Status:** Accepted
- **Date:** 2026-10-28

### Context
The maintenance flow between owner and mechanic (TS03 of the project report) goes through request, diagnostic and quote, budget approval or rejection, repair, and completion; invalid transitions must be rejected.

### Decision
`MaintenanceRequest` exposes one method per transition (`SubmitQuote`, `Approve`, `Reject`, `StartRepair`, `Finish`) and the API one endpoint per command (`diagnostic-and-quote`, `budget-decision`, `repair-start`, `completion`) instead of a generic status update. Invalid transitions return `409 Conflict`.

### Consequences
- (+) The aggregate guards the lifecycle and the API expresses intent.
- (−) More endpoints than a generic `PUT`.

---

## ADR-022 — Read other bounded contexts through context facades

- **Status:** Accepted
- **Date:** 2026-10-28

### Context
Report generation (TS04 of the project report) consolidates data owned by Fleet, Fuel, and Maintenance.

### Decision
Contexts read by others expose a facade contract in `Interfaces/ACL` (`IFleetContextFacade`, `IFuelContextFacade`, `IMaintenanceContextFacade`) implemented in `Application/ACL` with primitive types. Analytics depends only on these contracts.

### Consequences
- (+) Analytics does not depend on the domain model of other contexts.
- (−) Each new cross-context query requires a facade method.

---
