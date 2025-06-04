using System.Net.Mime;
using Asp.Versioning;
using JobMagnet.Application.Contracts.Commands.Profile;
using JobMagnet.Application.Contracts.Queries.Profile;
using JobMagnet.Application.Contracts.Responses.Profile;
using JobMagnet.Application.Mappers;
using JobMagnet.Application.UseCases.CvParser;
using JobMagnet.Application.UseCases.CvParser.Commands;
using JobMagnet.Domain.Core.Entities;
using JobMagnet.Domain.Ports.Repositories;
using JobMagnet.Domain.Ports.Repositories.Base;
using JobMagnet.Host.Controllers.Base;
using JobMagnet.Host.Mappers;
using JobMagnet.Host.ViewModels.Profile;
using Microsoft.AspNetCore.Mvc;

namespace JobMagnet.Host.Controllers.V1;

[ApiVersion("1")]
public class ProfileController(
    ICvParserHandler cvParser,
    ILogger<ProfileController> logger,
    IProfileQueryRepository queryRepository,
    IQueryRepository<PublicProfileIdentifierEntity, long> publicProfileRepository,
    ICommandRepository<ProfileEntity> commandRepository) : BaseController<ProfileController>(logger)
{
    [HttpPost]
    [ProducesResponseType(typeof(ProfileResponse), StatusCodes.Status201Created)]
    public async Task<IResult> CreateAsync([FromBody] ProfileCommand createCommand, CancellationToken cancellationToken)
    {
        var entity = createCommand.ToEntity();
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
        return Results.Ok(response);
    }

    [HttpGet("{id:long}", Name = nameof(GetProfileByIdAsync))]
    [ProducesResponseType(typeof(ProfileResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IResult> GetProfileByIdAsync(long id)
    {
        var entity = await queryRepository.GetByIdAsync(id);

        if (entity is null)
            return Results.NotFound();

        var responseModel = entity.ToModel();

        return Results.Ok(responseModel);
    }


    [HttpPut("{id:long}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IResult> PutAsync(int id, ProfileCommand command, CancellationToken cancellationToken)
    {
        var entity = await queryRepository.GetByIdAsync(id);

        if (entity is null)
            return Results.NotFound();

        entity.UpdateEntity(command);

        await commandRepository
            .Update(entity)
            .SaveChangesAsync(cancellationToken)
            .ConfigureAwait(false);

        return Results.NoContent();
    }

    [HttpGet]
    [ProducesResponseType(typeof(ProfileViewModel), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IResult> GetProfileAsync([FromQuery] ProfileQueryParameters queryParameters)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(queryParameters.ProfileSlug);

        var publicProfile = await publicProfileRepository
            .FirstOrDefaultAsync(x => x.ProfileSlugUrl == queryParameters.ProfileSlug)
            .ConfigureAwait(false);

        if (publicProfile is null)
            return Results.NotFound();

        var entity = await queryRepository
            .WhereCondition(x => x.Id == publicProfile.ProfileId)
            .WithResume()
            .WithSkills()
            .WithTalents()
            .WithPortfolioGallery()
            .WithSummary()
            .WithServices()
            .WithTestimonials()
            .BuildFirstOrDefaultAsync();

        if (entity is null)
            return Results.NotFound();

        var responseModel = entity.ToViewModel();

        return Results.Ok(responseModel);
    }
}