using FlotixPlatform.API.Commerce.Domain.Model.Aggregates;
using FlotixPlatform.API.Commerce.Domain.Model.Commands;
using FlotixPlatform.API.Commerce.Application.Errors;
using FlotixPlatform.API.Shared.Application.Patterns;

namespace FlotixPlatform.API.Commerce.Application.Services;

/// <summary>
///     Command service interface for IoT order operations.
/// </summary>
/// <remarks>
///     This interface defines the contract for handling the commands that change IoT orders.
/// </remarks>
public interface IIotOrderCommandService
{
    /// <summary>
    ///     Handle the create IoT order command.
    /// </summary>
    /// <param name="command">CreateIotOrderCommand command</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>
    ///     A Result containing the created IotOrder on success, or the error reason on failure.
    /// </returns>
    Task<Result<IotOrder, CreateIotOrderError>> Handle(CreateIotOrderCommand command, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Handle the update IoT order command.
    /// </summary>
    /// <param name="command">UpdateIotOrderCommand command</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>
    ///     A Result containing the updated IotOrder on success, or the error reason on failure.
    /// </returns>
    Task<Result<IotOrder, UpdateIotOrderError>> Handle(UpdateIotOrderCommand command, CancellationToken cancellationToken = default);
}
