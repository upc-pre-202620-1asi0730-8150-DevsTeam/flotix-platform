using System.Net.Mime;
using FlotixPlatform.Api.Iam.Application.CommandServices;
using FlotixPlatform.Api.Iam.Infrastructure.Pipeline.Middleware.Attributes;
using FlotixPlatform.Api.Iam.Interfaces.Rest.Resources;
using FlotixPlatform.Api.Iam.Interfaces.Rest.Transform;
using FlotixPlatform.Api.Resources.Errors;
using FlotixPlatform.Api.Shared.Interfaces.Rest.ProblemDetails;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace FlotixPlatform.Api.Iam.Interfaces.Rest;

[Authorize]
[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[Tags("Available Authentication and Token endpoints")]
public class TokensController(
    IUserCommandService userCommandService,
    IStringLocalizer<ErrorMessages> errorLocalizer,
    ProblemDetailsFactory problemDetailsFactory)
    : ControllerBase
{
    /**
     * <summary>
     *     Create token endpoint. Authenticates credentials and issues a JWT token.
     * </summary>
     * <param name="signInResource">The sign-in credentials containing username and password.</param>
     * <param name="cancellationToken">The cancellation token.</param>
     * <returns>The authenticated user resource, including a JWT token.</returns>
     */
    [HttpPost]
    [AllowAnonymous]
    [EndpointSummary("Create authentication token")]
    [EndpointDescription("Authenticates user credentials and issues a JWT bearer token")]
    [ProducesResponseType(typeof(AuthenticatedUserResource), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateToken([FromBody] SignInResource signInResource,
        CancellationToken cancellationToken)
    {
        var signInCommand = SignInCommandFromResourceAssembler.ToCommandFromResource(signInResource);
        var result = await userCommandService.Handle(signInCommand, cancellationToken);

        return IamActionResultAssembler.ToActionResultFromSignInResult(
            this,
            result,
            errorLocalizer,
            problemDetailsFactory,
            userAndToken =>
                Ok(AuthenticatedUserResourceFromEntityAssembler.ToResourceFromEntity(userAndToken.user,
                    userAndToken.token))
        );
    }
}
