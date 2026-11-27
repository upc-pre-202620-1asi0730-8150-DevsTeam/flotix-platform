using FlotixPlatform.Api.Notifications.Domain.Model.Aggregates;
using FlotixPlatform.Api.Notifications.Application.Errors;
using FlotixPlatform.Api.Notifications.Application.Services;
using FlotixPlatform.Api.Notifications.Domain.Model.Commands;
using FlotixPlatform.Api.Notifications.Domain.Repositories;
using FlotixPlatform.Api.Shared.Domain.Repositories;
using FlotixPlatform.Api.Shared.Application.Patterns;
using Microsoft.EntityFrameworkCore;

namespace FlotixPlatform.Api.Notifications.Application.Internal.CommandServices;

/// <summary>
///     Application service for handling notification commands.
/// </summary>
/// <remarks>
///     This service acts as the command handler for notifications.
///     It enforces the application rules, delegates the business rules to the aggregate,
///     then coordinates with the unit of work to persist changes.
///     Logs warnings for rejected commands and errors for persistence failures.
/// </remarks>
/// <param name="notificationRepository">Repository for accessing notification data.</param>
/// <param name="unitOfWork">Unit of work for managing transaction scope.</param>
/// <param name="logger">Logger for diagnostic and error reporting.</param>
/// See
/// <see cref="INotificationRepository">INotificationRepository</see>
/// ,
/// <see cref="IUnitOfWork">IUnitOfWork</see>
public class NotificationCommandService(
    INotificationRepository notificationRepository,
    IUnitOfWork unitOfWork,
    ILogger<NotificationCommandService> logger)
    : INotificationCommandService
{
    /// <inheritdoc />
    public async Task<Result<Notification, CreateNotificationError>> Handle(CreateNotificationCommand command,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var notification = new Notification(command);
            await notificationRepository.AddAsync(notification, cancellationToken);
            await unitOfWork.CompleteAsync(cancellationToken);
            return new Result<Notification, CreateNotificationError>.Success(notification);
        }
        catch (ArgumentException ex)
        {
            logger.LogWarning(ex, "Invalid arguments while creating notification");
            return new Result<Notification, CreateNotificationError>.Failure(CreateNotificationError.InvalidNotification);
        }
        catch (DbUpdateException ex) when (IsForeignKeyViolation(ex))
        {
            logger.LogWarning(ex, "Foreign key violation creating notification");
            return new Result<Notification, CreateNotificationError>.Failure(CreateNotificationError.ReferencedResourceNotFound);
        }
        catch (DbUpdateException ex)
        {
            logger.LogError(ex, "Database update failed creating notification");
            return new Result<Notification, CreateNotificationError>.Failure(CreateNotificationError.UnexpectedError);
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error creating notification");
            return new Result<Notification, CreateNotificationError>.Failure(CreateNotificationError.UnexpectedError);
        }
    }

    /// <inheritdoc />
    public async Task<Result<Notification, UpdateNotificationError>> Handle(UpdateNotificationCommand command,
        CancellationToken cancellationToken = default)
    {
        var notification = await notificationRepository.FindByIdAsync(command.NotificationId, cancellationToken);
        if (notification is null)
            return new Result<Notification, UpdateNotificationError>.Failure(
                UpdateNotificationError.NotificationNotFound);

        try
        {
            notification.UpdateInformation(command);
            notificationRepository.Update(notification);
            await unitOfWork.CompleteAsync(cancellationToken);
            return new Result<Notification, UpdateNotificationError>.Success(notification);
        }
        catch (ArgumentException ex)
        {
            logger.LogWarning(ex, "Invalid arguments while updating notification");
            return new Result<Notification, UpdateNotificationError>.Failure(UpdateNotificationError.InvalidNotification);
        }
        catch (DbUpdateException ex)
        {
            logger.LogError(ex, "Database update failed updating notification");
            return new Result<Notification, UpdateNotificationError>.Failure(UpdateNotificationError.UnexpectedError);
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error updating notification");
            return new Result<Notification, UpdateNotificationError>.Failure(UpdateNotificationError.UnexpectedError);
        }
    }

    /// <inheritdoc />
    public async Task<Result<Notification, DeleteNotificationError>> Handle(DeleteNotificationCommand command,
        CancellationToken cancellationToken = default)
    {
        var notification = await notificationRepository.FindByIdAsync(command.NotificationId, cancellationToken);
        if (notification is null)
            return new Result<Notification, DeleteNotificationError>.Failure(
                DeleteNotificationError.NotificationNotFound);

        try
        {
            notificationRepository.Remove(notification);
            await unitOfWork.CompleteAsync(cancellationToken);
            return new Result<Notification, DeleteNotificationError>.Success(notification);
        }
        catch (DbUpdateException ex)
        {
            logger.LogError(ex, "Database update failed deleting notification");
            return new Result<Notification, DeleteNotificationError>.Failure(DeleteNotificationError.UnexpectedError);
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error deleting notification");
            return new Result<Notification, DeleteNotificationError>.Failure(DeleteNotificationError.UnexpectedError);
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
