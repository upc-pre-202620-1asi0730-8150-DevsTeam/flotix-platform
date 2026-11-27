using FlotixPlatform.Api.DemoData.Application.Services;
using FlotixPlatform.Api.Iam.Application.CommandServices;
using FlotixPlatform.Api.Iam.Application.Internal.OutboundServices;
using FlotixPlatform.Api.Iam.Domain.Model;
using FlotixPlatform.Api.Iam.Domain.Model.Aggregates;
using FlotixPlatform.Api.Iam.Domain.Model.Commands;
using FlotixPlatform.Api.Iam.Domain.Model.Errors;
using FlotixPlatform.Api.Iam.Domain.Repositories;
using FlotixPlatform.Api.Shared.Application.Model;
using FlotixPlatform.Api.Shared.Domain.Repositories;

namespace FlotixPlatform.Api.Iam.Application.Internal.CommandServices;

/**
 * <summary>
 *     The user command service
 * </summary>
 * <remarks>
 *     This class is used to handle user commands
 * </remarks>
 */
public class UserCommandService(
    IUserRepository userRepository,
    ITokenService tokenService,
    IHashingService hashingService,
    IUnitOfWork unitOfWork,
    IDemoDataSeeder demoDataSeeder,
    ILogger<UserCommandService> logger)
    : IUserCommandService
{
    /**
     * <summary>
     *     Handle sign in command
     * </summary>
     * <param name="command">The sign in command</param>
     * <param name="cancellationToken">The cancellation token</param>
     * <returns>The authenticated user and the JWT token</returns>
     */
    public async Task<Result<(User user, string token)>> Handle(SignInCommand command,
        CancellationToken cancellationToken)
    {
        var user = await userRepository.FindByUsernameAsync(command.Username, cancellationToken);

        if (user == null || !hashingService.VerifyPassword(command.Password, user.PasswordHash))
            return Result<(User user, string token)>.Failure(IamErrors.InvalidCredentials, IamError.InvalidCredentials);

        var token = tokenService.GenerateToken(user);

        return Result<(User user, string token)>.Success((user, token));
    }

    /**
     * <summary>
     *     Handle sign up command
     * </summary>
     * <param name="command">The sign-up command</param>
     * <param name="cancellationToken">The cancellation token</param>
     * <returns>The created user result.</returns>
     */
    public async Task<Result<User>> Handle(SignUpCommand command, CancellationToken cancellationToken)
    {
        if (await userRepository.ExistsByUsernameAsync(command.Username, cancellationToken))
            return Result<User>.Failure(IamErrors.UsernameAlreadyTaken, IamError.UsernameAlreadyTaken);

        var hashedPassword = hashingService.HashPassword(command.Password);
        var user = new User(command.Username, hashedPassword);

        await userRepository.AddAsync(user, cancellationToken);
        await unitOfWork.CompleteAsync(cancellationToken);
        await SeedDemoDataAsync(user, cancellationToken);
        return Result<User>.Success(user);
    }

    /**
     * <summary>
     *     Generate the demo fleet (about three months of history) of a new account
     * </summary>
     * <remarks>
     *     A failure is logged and never prevents the account from being created.
     * </remarks>
     * <param name="user">The created user</param>
     * <param name="cancellationToken">The cancellation token</param>
     */
    private async Task SeedDemoDataAsync(User user, CancellationToken cancellationToken)
    {
        try
        {
            await demoDataSeeder.SeedForUserAsync(user.Id.Value, cancellationToken);
        }
        catch (Exception exception) when (exception is not OperationCanceledException)
        {
            logger.LogError(exception, "Demo data could not be generated for user {UserId}", user.Id.Value);
        }
    }
}