using FlotixPlatform.Api.Fleet.Domain.Model.Aggregates;
using FlotixPlatform.Api.Fleet.Application.Errors;
using FlotixPlatform.API.Fleet.Domain.Model.Commands;
using FlotixPlatform.Api.Shared.Application.Patterns;

namespace FlotixPlatform.Api.Fleet.Application.Services;

/// <summary>
///     Command service interface for vehicle operations.
/// </summary>
/// <remarks>
///     This interface defines the contract for handling the commands that change vehicles.
/// </remarks>
public interface IVehicleCommandService
{
    /// <summary>
    ///     Handle the create vehicle command.
    /// </summary>
    /// <param name="command">CreateVehicleCommand command</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>
    ///     A Result containing the created Vehicle on success, or the error reason on failure.
    /// </returns>
    Task<Result<Vehicle, CreateVehicleError>> Handle(CreateVehicleCommand command, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Handle the update vehicle command.
    /// </summary>
    /// <param name="command">UpdateVehicleCommand command</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>
    ///     A Result containing the updated Vehicle on success, or the error reason on failure.
    /// </returns>
    Task<Result<Vehicle, UpdateVehicleError>> Handle(UpdateVehicleCommand command, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Handle the delete vehicle command.
    /// </summary>
    /// <param name="command">DeleteVehicleCommand command</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>
    ///     A Result containing the deleted Vehicle on success, or the error reason on failure.
    /// </returns>
    Task<Result<Vehicle, DeleteVehicleError>> Handle(DeleteVehicleCommand command, CancellationToken cancellationToken = default);
}