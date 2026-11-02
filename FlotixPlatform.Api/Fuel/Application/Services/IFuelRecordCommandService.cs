using FlotixPlatform.API.Fuel.Domain.Model.Aggregates;
using FlotixPlatform.API.Fuel.Domain.Model.Commands;
using FlotixPlatform.API.Fuel.Application.Errors;
using FlotixPlatform.API.Shared.Application.Patterns;

namespace FlotixPlatform.Api.Fuel.Application.Services;

/// <summary>
///     Command service interface for fuel record operations.
/// </summary>
/// <remarks>
///     This interface defines the contract for handling the commands that change fuel records.
/// </remarks>
public interface IFuelRecordCommandService
{
    /// <summary>
    ///     Handle the create fuel record command.
    /// </summary>
    /// <param name="command">CreateFuelRecordCommand command</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>
    ///     A Result containing the created FuelRecord on success, or the error reason on failure.
    /// </returns>
    Task<Result<FuelRecord, CreateFuelRecordError>> Handle(CreateFuelRecordCommand command, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Handle the update fuel record command.
    /// </summary>
    /// <param name="command">UpdateFuelRecordCommand command</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>
    ///     A Result containing the updated FuelRecord on success, or the error reason on failure.
    /// </returns>
    Task<Result<FuelRecord, UpdateFuelRecordError>> Handle(UpdateFuelRecordCommand command, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Handle the delete fuel record command.
    /// </summary>
    /// <param name="command">DeleteFuelRecordCommand command</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>
    ///     A Result containing the deleted FuelRecord on success, or the error reason on failure.
    /// </returns>
    Task<Result<FuelRecord, DeleteFuelRecordError>> Handle(DeleteFuelRecordCommand command, CancellationToken cancellationToken = default);
}
