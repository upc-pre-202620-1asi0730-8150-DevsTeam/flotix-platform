using FlotixPlatform.Api.Notifications.Domain.Model.Aggregates;
using FlotixPlatform.Api.Notifications.Domain.Model.Commands;
using FlotixPlatform.Api.Notifications.Application.Errors;
using FlotixPlatform.Api.Shared.Application.Patterns;

namespace FlotixPlatform.Api.Notifications.Application.Services;

/// <summary>
///     Command service interface for alert operations.
/// </summary>
/// <remarks>
///     This interface defines the contract for handling the commands that change alerts.
/// </remarks>
public interface IAlertCommandService
{
    /// <summary>
    ///     Handle the create alert command.
    /// </summary>
    /// <param name="command">CreateAlertCommand command</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>
    ///     A Result containing the created Alert on success, or the error reason on failure.
    /// </returns>
    Task<Result<Alert, CreateAlertError>> Handle(CreateAlertCommand command, CancellationToken cancellationToken = default);
}
