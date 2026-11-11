# Contributing to Flotix Platform

Thank you for your interest in contributing to Flotix Platform. This document outlines the conventions and expectations for contributors so that everyone can collaborate smoothly.

Please take a moment to read this guide before opening your first pull request.

---

## Table of Contents

- [Code of Conduct](#code-of-conduct)
- [Getting Started](#getting-started)
- [Project Structure](#project-structure)
- [Branching Strategy (Git Flow)](#branching-strategy-git-flow)
- [Commit Messages (Conventional Commits)](#commit-messages-conventional-commits)
- [Pull Request Guidelines](#pull-request-guidelines)
- [Domain-Driven Design Conventions](#domain-driven-design-conventions)
- [Coding Standards](#coding-standards)
- [Testing](#testing)
- [Semantic Versioning & Releases](#semantic-versioning--releases)
- [Architecture Decision Records](#architecture-decision-records)
- [Reporting Issues](#reporting-issues)

---

## Code of Conduct

This project and everyone participating in it is governed by respect and professionalism. When contributing:

- Be kind and constructive in reviews and discussions.
- Accept constructive criticism gracefully.
- Focus on what is best for the community and the project.
- Report unacceptable behavior to the project maintainers.

---

## Getting Started

1. **Fork** the repository and **clone** your fork locally.
2. Add the upstream remote:
   ```bash
   git remote add upstream https://github.com/<org>/flotix-platform.git
   ```
3. Follow the setup instructions in [README.md](README.md) to restore dependencies, configure the connection string, and run the API.
4. Verify everything works by building the solution:
   ```bash
   dotnet build flotix-platform.sln
   ```

---

## Project Structure

Flotix Platform follows a layered Domain-Driven Design structure organized by bounded context. Each bounded context contains four layers:

```
FlotixPlatform.Api/
├── Fleet/                  # Vehicle & Fleet Management
│   ├── Domain/             # Aggregates, value objects, commands, queries, repository contracts
│   ├── Application/        # Service contracts, implementations, context facades, errors
│   ├── Infrastructure/     # EF Core persistence, technical adapters
│   └── Interfaces/         # REST controllers, resources, assemblers, SignalR hubs
├── Fuel/                   # Fuel Control
├── Maintenance/            # Maintenance Management
├── Incidents/              # Incident Management
├── Monitoring/             # Real-Time Monitoring / Fleet Tracking
├── Notifications/          # Alerts & Notifications
├── Commerce/               # Digital Experience / IoT Commerce
├── Analytics/              # Reporting & Analytics
└── Shared/                 # Cross-cutting abstractions (repository contracts, UoW, patterns)
```

**Dependency rule:** `Interfaces` and `Infrastructure` depend on `Application` and `Domain`. Dependencies always point inward — never the reverse.

For a visual overview, see the [class diagram](docs/class-diagram.puml) and the [architecture decision records](docs/adrs.md).

---

## Branching Strategy (Git Flow)

The project follows the [Git Flow](https://nvie.com/posts/a-successful-git-branching-model/) branching model.

| Branch | Purpose | Source | Target |
|--------|---------|--------|--------|
| `main` | Production-ready releases | — | — |
| `develop` | Integration branch for upcoming release | — | — |
| `feature/*` | New features or enhancements | `develop` | `develop` |
| `bugfix/*` | Non-critical bug fixes | `develop` | `develop` |
| `hotfix/*` | Critical production fixes | `main` | `main` + `develop` |
| `release/*` | Release preparation and final polish | `develop` | `main` + `develop` |

### Branch naming conventions

- **Features:** `feature/add-vehicle-registration-validation`
- **Bug fixes:** `bugfix/fix-duplicate-fuel-record-detection`
- **Hotfixes:** `hotfix/fix-critical-auth-token-expiry`
- **Releases:** `release/1.1.0`

### Workflow

1. Create a feature branch from `develop`:
   ```bash
   git checkout develop
   git pull origin develop
   git checkout -b feature/your-feature-name
   ```
2. Develop, commit, and push your changes.
3. Open a pull request against `develop`.
4. After review and approval, merge the PR into `develop`.

For production hotfixes, branch from `main`, merge the fix into both `main` and `develop`, and tag the release.

---

## Commit Messages (Conventional Commits)

All commits must follow the [Conventional Commits](https://www.conventionalcommits.org/en/v1.0.0/) specification. This enables automated changelog generation and clear commit history.

### Format

```
<type>(<scope>): <description>

[optional body]

[optional footer(s)]
```

### Types

| Type | Description | Example |
|------|-------------|---------|
| `feat` | A new feature | `feat(fleet): add vehicle registration validation` |
| `fix` | A bug fix | `fix(fuel): correct duplicate record detection` |
| `docs` | Documentation only | `docs: update API endpoint descriptions` |
| `style` | Code style changes (formatting, semicolons, etc.) | `style(maintenance): fix indentation in controller` |
| `refactor` | Code refactoring (no feature or fix) | `refactor(shared): simplify Result type pattern` |
| `perf` | Performance improvement | `perf(monitoring): optimize telemetry background processing` |
| `test` | Adding or updating tests | `test(fleet): add unit tests for vehicle query service` |
| `build` | Build system or dependency changes | `build: update EF Core to 10.0.13` |
| `ci` | CI/CD configuration changes | `ci: add GitHub Actions workflow for testing` |
| `chore` | Maintenance tasks | `chore: clean up unused imports` |

### Scope

Use the bounded context name as the scope when the change affects a specific context:

- `fleet`, `fuel`, `maintenance`, `incidents`, `monitoring`, `notifications`, `commerce`, `analytics`, `shared`

For cross-cutting changes, omit the scope or use `api`:

```
feat: add global exception handler
refactor(api): restructure dependency injection setup
```

### Examples

```
feat(fleet): add license plate uniqueness validation

Enforce unique license plates at the application layer
and return 409 Conflict on duplicates.

Closes #42
```

```
fix(monitoring): process telemetry events with correct timestamp

TelemetryData was using server time instead of the
device-reported timestamp, causing incorrect ordering.
```

```
docs: add contributing guidelines

Include Git Flow, Conventional Commits, DDD conventions,
and pull request checklist.
```

---

## Pull Request Guidelines

A well-structured pull request makes review faster and improves code quality.

### Before submitting

- [ ] Branch from `develop` (or `main` for hotfixes).
- [ ] Follow the [Conventional Commits](#commit-messages-conventional-commits) format.
- [ ] Ensure the solution builds without warnings:
  ```bash
  dotnet build flotix-platform.sln
  ```
- [ ] Run tests (if applicable):
  ```bash
  dotnet test
  ```
- [ ] Update or add documentation if the change affects public behavior.
- [ ] If you modified the domain model, create and commit an EF Core migration:
  ```bash
  dotnet tool restore
  cd FlotixPlatform.Api
  dotnet ef migrations add <MigrationName>
  ```
- [ ] Squash trivial commits (typos, WIP) before requesting review.

### Pull request title

Use Conventional Commits format for the PR title as well:

```
feat(fleet): add vehicle registration validation
```

### Pull request description

Include the following sections:

1. **What** — Brief description of the change.
2. **Why** — Context or motivation (link to the issue if applicable).
3. **How** — Technical approach, especially for complex changes.
4. **Testing** — How you verified the change.
5. **Screenshots** (if applicable) — For API changes, include Scalar screenshots or curl examples.

### Review process

- At least one maintainer approval is required before merging.
- Address all review comments before requesting re-review.
- Keep PRs focused — one feature or fix per PR.

---

## Domain-Driven Design Conventions

The codebase applies a lightweight DDD style. When contributing, follow these conventions:

### Bounded contexts

Each bounded context is self-contained. When adding functionality:

- Place new code inside the appropriate bounded context folder.
- If the feature spans multiple contexts, use **context facades** (`ACL` folders) to read across boundaries.
- Cross-cutting abstractions belong in `Shared`.

### Domain layer

- **Aggregates** — Root entities that enforce invariants (e.g., `Vehicle`, `FuelRecord`, `MaintenanceRequest`).
- **Value objects** — Immutable types with validation (e.g., `LicensePlate`, `SerialNumber`, `EmailAddress`).
- **Commands** — Immutable records representing write intent (e.g., `CreateVehicleCommand`).
- **Queries** — Immutable records representing read intent (e.g., `GetVehicleByIdQuery`).
- Keep business rules close to the model. Validate value objects in their constructors.

### Application layer

- **Command services** — Handle commands and return `Result<TValue, TError>`.
- **Query services** — Handle queries and return domain entities or DTOs.
- Define application-specific error enums (e.g., `CreateVehicleError`).
- Accept `CancellationToken` on all handlers.

### Interfaces layer

- **Controllers** — Expose REST endpoints under `/api/v1/{resource}` using kebab-case routes.
- **Resources** — Request/response DTOs, separate from domain models.
- **Assemblers** — Static methods that translate between resources, commands, and results.
- **SignalR hubs** — Real-time event publishing.

### Infrastructure layer

- **EF Core** — One schema per bounded context with `snake_case` naming and plural table names.
- **Repositories** — Implement repository contracts from the domain layer.
- **Migrations** — Apply on startup via `Database.Migrate()`.

### Key patterns

- **CQS** — Separate command and query services per aggregate.
- **Result type** — Model expected failures with `Result<TValue, TError>`, not exceptions.
- **Context facades** — Read other bounded contexts through facade interfaces.

Refer to the [Architecture Decision Records](docs/adrs.md) for the rationale behind each decision.

---

## Coding Standards

### General

- Follow the existing code style of the module you are editing.
- Use C# 14 features and `.NET 10` APIs where appropriate.
- Enable nullable reference types (`<Nullable>enable</Nullable>`).
- Use `ILogger<T>` for structured logging.

### Naming

- **Classes and types:** `PascalCase` (e.g., `VehicleCommandService`).
- **Methods and properties:** `PascalCase` (e.g., `HandleAsync`).
- **Private fields:** `_camelCase` (e.g., `_vehicleRepository`).
- **Routes:** lowercase kebab-case (e.g., `/api/v1/fuel-records`).
- **Database identifiers:** `snake_case` with plural table names (handled by EF Core naming strategy).

### API conventions

- API base route: `/api/v1/{resource}`.
- Return `400 Bad Request` for validation failures with localized messages.
- Return `404 Not Found` for missing resources.
- Return `409 Conflict` for duplicates or invalid state transitions.
- Return `500 Internal Server Error` as RFC 7807 `ProblemDetails` for unexpected errors.
- Use `.resx` resources for localization (`en`, `en-US`, `es`, `es-PE`).

### Secrets

- Never commit secrets, API keys, or real connection strings.
- Use `.env` (git-ignored) for local development or `dotnet user-secrets`.
- Production secrets are injected as environment variables.
- See [README.md](README.md#connecting-to-neon-postgresql-and-keeping-secrets-out-of-git) for details.

---

## Testing

Write tests proportional to the complexity and risk of the change:

- **Bug fixes:** Add a reproduction test that demonstrates the issue before applying the fix.
- **New features:** Cover core logic paths and edge cases.
- **Refactoring:** Rely on existing tests; add coverage only if gaps are identified.

Run tests with:

```bash
dotnet test
```

For domain logic, prefer unit tests that exercise aggregates and value objects without ASP.NET Core or EF Core.

---

## Semantic Versioning & Releases

The project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html) (SemVer 2.0.0).

### Version format

```
MAJOR.MINOR.PATCH
```

| Version part | Increment when... |
|-------------|-------------------|
| **MAJOR** | Incompatible API changes are introduced. |
| **MINOR** | Backward-compatible functionality is added. |
| **PATCH** | Backward-compatible bug fixes are made. |

### Changelog

All notable changes are documented in [CHANGELOG.md](CHANGELOG.md), following the [Keep a Changelog](https://keepachangelog.com/en/1.1.0/) format.

When preparing a release:

1. Create a `release/X.Y.Z` branch from `develop`.
2. Update [CHANGELOG.md](CHANGELOG.md): move entries from `[Unreleased]` to the new version section with today's date.
3. Verify the build and run the full test suite.
4. Open a PR against `main`.
5. After merge, tag the commit:
   ```bash
   git tag -a v1.1.0 -m "Release v1.1.0"
   git push origin v1.1.0
   ```
6. Merge the release branch back into `develop`.

---

## Architecture Decision Records

Significant architectural decisions are recorded in [docs/adrs.md](docs/adrs.md). When making a new architectural decision:

1. Read existing ADRs to avoid duplication or conflict.
2. Add a new record with the next sequential number.
3. Include **Context**, **Decision**, and **Consequences**.
4. Set status to `Proposed`, then update to `Accepted` after review.
5. If a decision is replaced, mark the old ADR as `Superseded by ADR-XXX` and add a new one.

---

## Reporting Issues

Found a bug or have a feature request? Please open an issue on the repository.

### Bug reports

Include:

- **Steps to reproduce** — Clear, minimal instructions.
- **Expected behavior** — What should happen.
- **Actual behavior** — What actually happens, including error messages.
- **Environment** — .NET SDK version, PostgreSQL version, OS.

### Feature requests

Include:

- **Use case** — Who needs this and why.
- **Proposed solution** — How you envision it working.
- **Alternatives considered** — Other approaches you evaluated.

---

## Thank You

Your contributions make Flotix Platform better for everyone. We appreciate your time and effort.
