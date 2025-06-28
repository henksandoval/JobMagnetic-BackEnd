using System.Net.Mime;
using Asp.Versioning;
using JobMagnet.Application.Contracts.Commands.Profile;
using JobMagnet.Application.Contracts.Queries.Profile;
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
    IQueryRepository<VanityUrl, long> publicProfileRepository,
    ICommandRepository<Profile> commandRepository) : BaseController<ProfileController>(logger)
{
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
        await commandRepository.CreateAsync(entity, cancellationToken);
        await commandRepository.SaveChangesAsync(cancellationToken);
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
        throw new NotImplementedException();
//TODO: Implement the logic to update an existing Profile entity with the provided command.
/*
        var entity = await queryRepository.GetByIdAsync(new ProfileId(id), cancellationToken);

        if (entity is null)
            return Results.NotFound();

        entity.UpdateEntity(command);

        await commandRepository
            .Update(entity)
            .SaveChangesAsync(cancellationToken)
            .ConfigureAwait(false);

        return Results.NoContent();
*/
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
}