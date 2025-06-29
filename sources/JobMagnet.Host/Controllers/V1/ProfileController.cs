using System.Net.Mime;
using Asp.Versioning;
using JobMagnet.Application.Contracts.Commands.Portfolio;
using JobMagnet.Application.Contracts.Commands.Profile;
using JobMagnet.Application.Contracts.Queries.Profile;
using JobMagnet.Application.Contracts.Responses.Portfolio;
using JobMagnet.Application.Contracts.Responses.Profile;
using JobMagnet.Application.Mappers;
using JobMagnet.Application.UseCases.CvParser;
using JobMagnet.Application.UseCases.CvParser.Commands;
using JobMagnet.Domain.Aggregates.Profiles;
using JobMagnet.Domain.Aggregates.Profiles.Entities;
using JobMagnet.Domain.Ports.Repositories;
using JobMagnet.Domain.Ports.Repositories.Base;
using JobMagnet.Host.Controllers.Base;
using JobMagnet.Host.Mappers;
using JobMagnet.Host.ViewModels.Profile;
using JobMagnet.Shared.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace JobMagnet.Host.Controllers.V1;

[ApiVersion("1")]
public class ProfileController(
    IGuidGenerator guidGenerator,
    IClock clock,
    ICvParserHandler cvParser,
    ILogger<ProfileController> logger,
    IProfileQueryRepository queryRepository,
    ICommandRepository<Project> projectCommandRepository,
    IUnitOfWork unitOfWork,
    IQueryRepository<VanityUrl, long> publicProfileRepository,
    ICommandRepository<Profile> profileCommandRepository) : BaseController<ProfileController>(logger)
{
    #region Profile
    [HttpPost]
    [ProducesResponseType(typeof(ProfileResponse), StatusCodes.Status201Created)]
    public async Task<IResult> CreateAsync([FromBody] ProfileCommand createCommand, CancellationToken cancellationToken)
    {
        var data = createCommand.ProfileData;
        var entity = Profile.CreateInstance(
            guidGenerator,
            clock,
            data.FirstName,
            data.LastName,
            data.ProfileImageUrl,
            data.BirthDate,
            data.MiddleName,
            data.SecondLastName
        );
        await profileCommandRepository.CreateAsync(entity, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        var newRecord = entity.ToModel();

        return Results.CreatedAtRoute(nameof(GetProfileByIdAsync), new { id = newRecord.Id }, newRecord);
    }

    [HttpPost]
    [Route("create-from-cv")]
    [Consumes(MediaTypeNames.Multipart.FormData)]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IResult> CreateAsync(IFormFile cvFile, CancellationToken cancellationToken)
    {
        var command = new CvParserCommand(cvFile.OpenReadStream(), cvFile.FileName, cvFile.ContentType);
        var response = await cvParser.ParseAsync(command, cancellationToken).ConfigureAwait(false);
        return Results.CreatedAtRoute(nameof(GetProfileByIdAsync), new { id = response.ProfileId }, response);
    }

    [HttpGet("{id:guid}", Name = nameof(GetProfileByIdAsync))]
    [ProducesResponseType(typeof(ProfileResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IResult> GetProfileByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var entity = await queryRepository.GetByIdAsync(new ProfileId(id), cancellationToken);

        if (entity is null)
            return Results.NotFound();

        var responseModel = entity.ToModel();

        return Results.Ok(responseModel);
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IResult> PutAsync(Guid id, ProfileCommand command, CancellationToken cancellationToken)
    {
        var entity = await queryRepository.GetByIdAsync(new ProfileId(id), cancellationToken);

        if (entity is null)
            return Results.NotFound();

        var data = command.ProfileData;
        entity.Update(
            data.FirstName,
            data.LastName,
            data.MiddleName,
            data.SecondLastName,
            data.BirthDate,
            data.ProfileImageUrl
        );

        await unitOfWork
            .SaveChangesAsync(cancellationToken)
            .ConfigureAwait(false);

        return Results.NoContent();
    }

    [HttpGet]
    [ProducesResponseType(typeof(ProfileViewModel), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IResult> GetProfileAsync([FromQuery] ProfileQueryParameters queryParameters, CancellationToken cancellationToken)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(queryParameters.ProfileSlug);

        var publicProfile = await publicProfileRepository
            .FirstOrDefaultAsync(x => x.ProfileSlugUrl == queryParameters.ProfileSlug, cancellationToken)
            .ConfigureAwait(false);

        if (publicProfile is null)
            return Results.NotFound();

        var entity = await queryRepository
            .WhereCondition(x => x.Id == publicProfile.ProfileId)
            .WithResume()
            .WithSkills()
            .WithTalents()
            .WithProject()
            .WithSummary()
            .WithTestimonials()
            .BuildFirstOrDefaultAsync();

        if (entity is null)
            return Results.NotFound();

        var responseModel = entity.ToViewModel();

        return Results.Ok(responseModel);
    }

    #endregion

    #region Project

    [HttpPost("{profileId:guid}/project")]
    [ProducesResponseType(typeof(ProjectResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IResult> AddProjectToProfileAsync(Guid profileId, [FromBody] ProjectCommand command, CancellationToken cancellationToken)
    {
        if (profileId != command.ProjectData?.ProfileId)
            throw new ArgumentException($"{nameof(command.ProjectData.ProfileId)} must be equal to profileId.");

        var profile = await queryRepository
            .WhereCondition(p => p.Id == new ProfileId(profileId))
            .WithProject()
            .BuildFirstOrDefaultAsync()
            .ConfigureAwait(false);

        if (profile is null)
            return Results.NotFound();

        var project = profile.Portfolio.AddProject(
            guidGenerator,
            clock,
            command.ProjectData.Title ?? string.Empty,
            command.ProjectData.Description ?? string.Empty,
            command.ProjectData.UrlLink ?? string.Empty,
            command.ProjectData.UrlImage ?? string.Empty,
            command.ProjectData.UrlVideo ?? string.Empty,
            command.ProjectData.Type ?? string.Empty
        );

        await projectCommandRepository.CreateAsync(project, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        var result = project.ToModel();

        return Results.CreatedAtRoute("GetProjectsByProfile", new { profileId }, result);
    }

    [HttpGet("{profileId:guid}/projects", Name = "GetProjectsByProfile")]
    [ProducesResponseType(typeof(IEnumerable<ProjectResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IResult> GetProjectsByProfileAsync(Guid profileId, CancellationToken cancellationToken)
    {
        var profile = await queryRepository
            .WhereCondition(p => p.Id == new ProfileId(profileId))
            .WithProject()
            .BuildFirstOrDefaultAsync()
            .ConfigureAwait(false);

        if (profile is null)
        {
            return Results.NotFound("Profile not found.");
        }

        var response = profile.Projects
            .Select(project => project.ToModel())
            .ToList();

        return Results.Ok(response);
    }

    #endregion
}