using FlotixPlatform.Api.Iam.Domain.Model.Aggregates;
using FlotixPlatform.Api.Iam.Domain.Model.Commands;
using FlotixPlatform.Api.Shared.Application.Model;

namespace FlotixPlatform.Api.Iam.Application.CommandServices;

/**
 * <summary>
 *     The user command service
 * </summary>
 * <remarks>
 *     This interface is used to handle user commands
 * </remarks>
 */
public interface IUserCommandService
{
    /**
        * <summary>
        *     Handle sign in command
        * </summary>
        * <param name="command">The sign in command</param>
        * <param name="cancellationToken">The cancellation token</param>
        * <returns>The authenticated user and the JWT token</returns>
        */
    Task<Result<(User user, string token)>> Handle(SignInCommand command, CancellationToken cancellationToken);

    /**
        * <summary>
        *     Handle sign up command
        * </summary>
        * <param name="command">The sign-up command</param>
        * <param name="cancellationToken">The cancellation token</param>
        * <returns>The created user result.</returns>
        */
    Task<Result<User>> Handle(SignUpCommand command, CancellationToken cancellationToken);
}