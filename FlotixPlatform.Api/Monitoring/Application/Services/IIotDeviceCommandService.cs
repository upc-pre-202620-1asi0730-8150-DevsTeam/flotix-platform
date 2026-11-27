using FlotixPlatform.Api.Monitoring.Domain.Model.Aggregates;
using FlotixPlatform.Api.Monitoring.Domain.Model.Commands;
using FlotixPlatform.Api.Monitoring.Application.Errors;
using FlotixPlatform.Api.Shared.Application.Patterns;

namespace FlotixPlatform.Api.Monitoring.Application.Services;

/// <summary>
///     Command service interface for IoT device operations.
/// </summary>
/// <remarks>
///     This interface defines the contract for handling the commands that change IoT devices.
/// </remarks>
public interface IIotDeviceCommandService
{
    /// <summary>
    ///     Handle the create IoT device command.
    /// </summary>
    /// <param name="command">CreateIotDeviceCommand command</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>
    ///     A Result containing the created IotDevice on success, or the error reason on failure.
    /// </returns>
    Task<Result<IotDevice, CreateIotDeviceError>> Handle(CreateIotDeviceCommand command, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Handle the update IoT device command.
    /// </summary>
    /// <param name="command">UpdateIotDeviceCommand command</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>
    ///     A Result containing the updated IotDevice on success, or the error reason on failure.
    /// </returns>
    Task<Result<IotDevice, UpdateIotDeviceError>> Handle(UpdateIotDeviceCommand command, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Handle the delete IoT device command.
    /// </summary>
    /// <param name="command">DeleteIotDeviceCommand command</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>
    ///     A Result containing the deleted IotDevice on success, or the error reason on failure.
    /// </returns>
    Task<Result<IotDevice, DeleteIotDeviceError>> Handle(DeleteIotDeviceCommand command, CancellationToken cancellationToken = default);
}
