using FlotixPlatform.Api.Iam.Domain.Model;
using FlotixPlatform.Api.Iam.Domain.Model.Aggregates;
using FlotixPlatform.Api.Resources.Errors;
using FlotixPlatform.Api.Shared.Application.Model;
using FlotixPlatform.Api.Shared.Interfaces.Rest.ProblemDetails;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

// For IamError
// For ProblemDetailsFactory

// For StatusCodes

namespace FlotixPlatform.Api.Iam.Interfaces.Rest.Transform;

public static class IamActionResultAssembler
{
    // --- Helper for transforming IamError to StatusCode ---
    private static int ToStatusCodeFromIamError(IamError error)
    {
        return error switch
        {
            IamError.InvalidCredentials => StatusCodes.Status400BadRequest,
            IamError.UsernameAlreadyTaken => StatusCodes.Status409Conflict,
            IamError.OperationCancelled => StatusCodes.Status409Conflict,
            IamError.DatabaseError => StatusCodes.Status500InternalServerError,
            IamError.InternalServerError => StatusCodes.Status500InternalServerError,
            IamError.ExternalServiceError => StatusCodes
                .Status503ServiceUnavailable, // Assuming 503 for external service issues
            _ => StatusCodes.Status400BadRequest // Default
        };
    }

    // --- Specific Assembler Methods ---

    public static IActionResult ToActionResultFromSignInResult(
        ControllerBase controller,
        Result<(User user, string token)> result,
        IStringLocalizer<ErrorMessages> errorLocalizer,
        ProblemDetailsFactory problemDetailsFactory,
        Func<(User user, string token), IActionResult> successAction)
    {
        if (result.IsSuccess) return successAction(result.Value!);

        var statusCode = ToStatusCodeFromIamError((IamError)result.Error!);
        return problemDetailsFactory.CreateProblemDetails(controller, statusCode, result.Error, result.Message);
    }

    public static IActionResult ToActionResultFromSignUpResult(
        ControllerBase controller,
        Result<User> result,
        IStringLocalizer<ErrorMessages> errorLocalizer,
        ProblemDetailsFactory problemDetailsFactory,
        Func<User, IActionResult> successAction)
    {
        if (result.IsSuccess) return successAction(result.Value!);

        var statusCode = ToStatusCodeFromIamError((IamError)result.Error!);
        return problemDetailsFactory.CreateProblemDetails(controller, statusCode, result.Error, result.Message);
    }

    public static IActionResult ToActionResultFromGetUserByIdResult(
        ControllerBase controller,
        User? user, // Direct user object, not a Result
        IStringLocalizer<ErrorMessages> errorLocalizer,
        ProblemDetailsFactory problemDetailsFactory,
        Func<User, IActionResult> successAction)
    {
        if (user is null)
            return problemDetailsFactory.CreateProblemDetails(
                controller,
                ToStatusCodeFromIamError(IamError.UserNotFound),
                IamError.UserNotFound,
                errorLocalizer[nameof(IamError.UserNotFound)]
            );
        return successAction(user);
    }
}