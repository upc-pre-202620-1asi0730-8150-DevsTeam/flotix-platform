using FlotixPlatform.Api.Analytics.Domain.Model.Aggregates;
using FlotixPlatform.Api.Analytics.Application.Errors;
using FlotixPlatform.Api.Analytics.Application.Services;
using FlotixPlatform.Api.Analytics.Domain.Model.Commands;
using FlotixPlatform.Api.Analytics.Domain.Model.ValueObjects;
using FlotixPlatform.Api.Analytics.Domain.Repositories;
using FlotixPlatform.Api.Fleet.Interfaces.ACL;
using FlotixPlatform.Api.Fuel.Interfaces.ACL;
using FlotixPlatform.Api.Maintenance.Interfaces.ACL;
using FlotixPlatform.Api.Shared.Domain.Repositories;
using FlotixPlatform.Api.Shared.Application.Patterns;
using Microsoft.EntityFrameworkCore;

namespace FlotixPlatform.Api.Analytics.Application.Internal.CommandServices;

/// <summary>
///     Application service for handling report commands.
/// </summary>
/// <remarks>
///     This service acts as the command handler for reports. It generates fuel and maintenance reports with the
///     data that the Fleet, Fuel and Maintenance contexts expose through their facades (anti-corruption layer).
///     It enforces the application rules, delegates the business rules to the aggregate,
///     then coordinates with the unit of work to persist changes.
///     Logs warnings for rejected commands and errors for persistence failures.
/// </remarks>
/// <param name="reportRepository">Repository for accessing report data.</param>
/// <param name="fleetContextFacade">Facade of the Vehicle &amp; Fleet Management context (vehicles of the owner).</param>
/// <param name="fuelContextFacade">Facade of the Fuel Control context (refuels).</param>
/// <param name="maintenanceContextFacade">Facade of the Maintenance Management context (maintenance requests).</param>
/// <param name="unitOfWork">Unit of work for managing transaction scope.</param>
/// <param name="logger">Logger for diagnostic and error reporting.</param>
/// See
/// <see cref="IReportRepository">IReportRepository</see>
/// ,
/// <see cref="IUnitOfWork">IUnitOfWork</see>
public class ReportCommandService(
    IReportRepository reportRepository,
    IFleetContextFacade fleetContextFacade,
    IFuelContextFacade fuelContextFacade,
    IMaintenanceContextFacade maintenanceContextFacade,
    IUnitOfWork unitOfWork,
    ILogger<ReportCommandService> logger)
    : IReportCommandService
{
    /// <inheritdoc />
    public async Task<Result<(Report Report, IReadOnlyList<ReportLine> Lines), GenerateReportError>> Handle(
        GenerateReportCommand command, CancellationToken cancellationToken = default)
    {
        try
        {
            var report = new Report(command);
            var lines = await GenerateLinesAsync(command, cancellationToken);
            await reportRepository.AddAsync(report, cancellationToken);
            await unitOfWork.CompleteAsync(cancellationToken);
            return new Result<(Report Report, IReadOnlyList<ReportLine> Lines), GenerateReportError>.Success((report, lines));
        }
        catch (ArgumentException ex)
        {
            logger.LogWarning(ex, "Invalid arguments while generating report");
            return new Result<(Report Report, IReadOnlyList<ReportLine> Lines), GenerateReportError>.Failure(GenerateReportError.InvalidReport);
        }
        catch (DbUpdateException ex) when (IsForeignKeyViolation(ex))
        {
            logger.LogWarning(ex, "Foreign key violation generating report");
            return new Result<(Report Report, IReadOnlyList<ReportLine> Lines), GenerateReportError>.Failure(GenerateReportError.ReferencedResourceNotFound);
        }
        catch (DbUpdateException ex)
        {
            logger.LogError(ex, "Database update failed generating report");
            return new Result<(Report Report, IReadOnlyList<ReportLine> Lines), GenerateReportError>.Failure(GenerateReportError.UnexpectedError);
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error generating report");
            return new Result<(Report Report, IReadOnlyList<ReportLine> Lines), GenerateReportError>.Failure(GenerateReportError.UnexpectedError);
        }
    }

    /// <summary>
    ///     Consolidates the figures of each vehicle of the owner within the period (ConsolidateFleetData).
    /// </summary>
    /// <remarks>
    ///     Fuel reports add up the refuels, liters and fuel cost; maintenance reports add up the requests, the
    ///     completed repairs and the approved budgets. Vehicles without records in the period are not listed.
    /// </remarks>
    /// <param name="command">The GenerateReportCommand command</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>The figures of each vehicle ordered by license plate; empty when there are no records.</returns>
    private async Task<IReadOnlyList<ReportLine>> GenerateLinesAsync(GenerateReportCommand command,
        CancellationToken cancellationToken)
    {
        var licensePlates = await fleetContextFacade.FetchLicensePlatesByOwnerIdAsync(command.OwnerId, cancellationToken);
        if (licensePlates.Count == 0) return [];

        var vehicleIds = licensePlates.Keys.ToList();
        var from = new DateTimeOffset(command.From.ToDateTime(TimeOnly.MinValue), TimeSpan.Zero);
        var to = new DateTimeOffset(command.To.AddDays(1).ToDateTime(TimeOnly.MinValue), TimeSpan.Zero);

        IEnumerable<ReportLine> lines = command.ReportType switch
        {
            ReportType.FuelConsumption =>
                (await fuelContextFacade.FetchFuelRecordsAsync(vehicleIds, from, to, cancellationToken))
                .GroupBy(record => record.VehicleId)
                .Select(records => new ReportLine(records.Key, licensePlates[records.Key], records.Count(),
                    records.Sum(record => record.Liters), records.Sum(record => record.Cost))),
            ReportType.MaintenanceCost =>
                (await maintenanceContextFacade.FetchMaintenanceRequestsAsync(vehicleIds, from, to, cancellationToken))
                .GroupBy(request => request.VehicleId)
                .Select(requests => new ReportLine(requests.Key, licensePlates[requests.Key], requests.Count(),
                    requests.Count(request => request.IsCompleted),
                    requests.Where(request => request.IsBudgetApproved).Sum(request => request.EstimatedCost))),
            _ => throw new ArgumentOutOfRangeException(nameof(command), "The report type is not supported.")
        };
        return lines.OrderBy(line => line.LicensePlate).ToList();
    }

    /// <inheritdoc />
    public async Task<Result<Report, DeleteReportError>> Handle(DeleteReportCommand command,
        CancellationToken cancellationToken = default)
    {
        var report = await reportRepository.FindByIdAsync(command.ReportId, cancellationToken);
        if (report is null)
            return new Result<Report, DeleteReportError>.Failure(
                DeleteReportError.ReportNotFound);

        try
        {
            reportRepository.Remove(report);
            await unitOfWork.CompleteAsync(cancellationToken);
            return new Result<Report, DeleteReportError>.Success(report);
        }
        catch (DbUpdateException ex)
        {
            logger.LogError(ex, "Database update failed deleting report");
            return new Result<Report, DeleteReportError>.Failure(DeleteReportError.UnexpectedError);
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error deleting report");
            return new Result<Report, DeleteReportError>.Failure(DeleteReportError.UnexpectedError);
        }
    }

    /// <summary>
    /// Determines whether a DbUpdateException represents a foreign key constraint violation.
    /// </summary>
    /// <param name="exception">The exception to inspect.</param>
    /// <returns>True if the exception is due to a PostgreSQL foreign key violation (SQLSTATE 23503), false otherwise.</returns>
    private static bool IsForeignKeyViolation(DbUpdateException exception)
    {
        for (Exception? current = exception; current is not null; current = current.InnerException)
        {
            if (!string.Equals(current.GetType().Name, "PostgresException", StringComparison.Ordinal)) continue;
            var sqlStateProperty = current.GetType().GetProperty("SqlState");
            if (sqlStateProperty?.GetValue(current) is "23503")
                return true;
        }
        return false;
    }
}
