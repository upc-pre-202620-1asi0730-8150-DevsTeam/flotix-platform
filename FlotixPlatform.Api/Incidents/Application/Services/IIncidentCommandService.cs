using FlotixPlatform.Api.Incidents.Domain.Model.Aggregates;
using FlotixPlatform.Api.Incidents.Domain.Model.Commands;
using FlotixPlatform.Api.Incidents.Application.Errors;
using FlotixPlatform.Api.Shared.Application.Patterns;

namespace FlotixPlatform.Api.Incidents.Application.Services;

/// <summary>
///     Command service interface for incident operations.
/// </summary>
/// <remarks>
///     This interface defines the contract for handling the commands that change incidents.
/// </remarks>
public interface IIncidentCommandService
{
    /// <summary>
    ///     Handle the create incident command.
    /// </summary>
    /// <param name="command">CreateIncidentCommand command</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>
    ///     A Result containing the created Incident on success, or the error reason on failure.
    /// </returns>
    Task<Result<Incident, CreateIncidentError>> Handle(CreateIncidentCommand command, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Handle the update incident command.
    /// </summary>
    /// <param name="command">UpdateIncidentCommand command</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>
    ///     A Result containing the updated Incident on success, or the error reason on failure.
    /// </returns>
    Task<Result<Incident, UpdateIncidentError>> Handle(UpdateIncidentCommand command, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Handle the delete incident command.
    /// </summary>
    /// <param name="command">DeleteIncidentCommand command</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>
    ///     A Result containing the deleted Incident on success, or the error reason on failure.
    /// </returns>
    Task<Result<Incident, DeleteIncidentError>> Handle(DeleteIncidentCommand command, CancellationToken cancellationToken = default);
}
