using FlotixPlatform.Api.Shared.Domain.Model;
using FlotixPlatform.Api.Shared.Domain.Model.Errors;

namespace FlotixPlatform.Api.Iam.Domain.Model.Errors;

public static class IamErrors
{
    public static readonly Error InvalidCredentials =
        Error.Unauthorized("Iam.InvalidCredentials", "Invalid username or password.");

    public static readonly Error UsernameAlreadyTaken =
        Error.Conflict("Iam.UsernameAlreadyTaken", "The specified username is already taken.");

    public static readonly Error UserNotFound =
        Error.NotFound("Iam.UserNotFound", "The specified user was not found.");

    public static readonly Error UserCreationFailed =
        Error.Failure("Iam.UserCreationFailed", "An error occurred while creating the user.");
}
