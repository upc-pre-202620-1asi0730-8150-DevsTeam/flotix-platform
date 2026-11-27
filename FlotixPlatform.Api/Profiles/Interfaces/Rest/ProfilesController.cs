using System.Net.Mime;
using FlotixPlatform.Api.Profiles.Application.CommandServices;
using FlotixPlatform.Api.Profiles.Application.QueryServices;
using FlotixPlatform.Api.Profiles.Domain.Model.Queries;
using FlotixPlatform.Api.Profiles.Interfaces.Rest.Resources;
using FlotixPlatform.Api.Profiles.Interfaces.Rest.Transform;
using FlotixPlatform.Api.Resources.Errors;
using FlotixPlatform.Api.Shared.Interfaces.Rest.ProblemDetails;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace FlotixPlatform.Api.Profiles.Interfaces.Rest;

[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[Tags("Available Profile Endpoints.")]
public class ProfilesController(
    IProfileCommandService profileCommandService,
    IProfileQueryService profileQueryService,
    IStringLocalizer<ErrorMessages> errorLocalizer,
    ProblemDetailsFactory problemDetailsFactory)
    : ControllerBase
{
    private readonly IStringLocalizer<ErrorMessages> _errorLocalizer = errorLocalizer;
    private readonly ProblemDetailsFactory _problemDetailsFactory = problemDetailsFactory;

    [HttpGet("{profileId:int}")]
    [EndpointSummary("Get Profile by Id")]
    [EndpointDescription("Get a profile by its unique identifier.")]
    [ProducesResponseType(typeof(ProfileResource), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetProfileById(int profileId, CancellationToken cancellationToken)
    {
        var getProfileByIdQuery = new GetProfileByIdQuery(profileId);
        var profile = await profileQueryService.Handle(getProfileByIdQuery, cancellationToken);

        return ProfilesActionResultAssembler.ToActionResultFromGetProfileByIdResult(
            this,
            profile,
            _errorLocalizer,
            _problemDetailsFactory,
            foundProfile => Ok(ProfileResourceFromEntityAssembler.ToResourceFromEntity(foundProfile))
        );
    }

    [HttpPost]
    [EndpointSummary("Create Profile")]
    [EndpointDescription("Create a new profile of an owner, a driver or a mechanic.")]
    [ProducesResponseType(typeof(ProfileResource), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> CreateProfile([FromBody] CreateProfileResource resource,
        CancellationToken cancellationToken)
    {
        var createProfileCommand = CreateProfileCommandFromResourceAssembler.ToCommandFromResource(resource);
        var result = await profileCommandService.Handle(createProfileCommand, cancellationToken);

        return ProfilesActionResultAssembler.ToActionResultFromCreateProfileResult(
            this,
            result,
            _errorLocalizer,
            _problemDetailsFactory,
            createdProfile => CreatedAtAction(nameof(GetProfileById), new { profileId = createdProfile.Id.Value },
                ProfileResourceFromEntityAssembler.ToResourceFromEntity(createdProfile))
        );
    }

    [HttpGet]
    [EndpointSummary("Get All Profiles")]
    [EndpointDescription("Get all profiles.")]
    [ProducesResponseType(typeof(IEnumerable<ProfileResource>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllProfiles(CancellationToken cancellationToken)
    {
        var getAllProfilesQuery = new GetAllProfilesQuery();
        var profiles = await profileQueryService.Handle(getAllProfilesQuery, cancellationToken);
        var profileResources = profiles.Select(ProfileResourceFromEntityAssembler.ToResourceFromEntity);
        return Ok(profileResources);
    }
}
