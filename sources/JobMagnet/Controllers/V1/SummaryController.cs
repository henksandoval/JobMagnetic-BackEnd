using Asp.Versioning;
using JobMagnet.Controllers.Base;
using JobMagnet.Infrastructure.Entities;
using JobMagnet.Infrastructure.Repositories.Base.Interfaces;
using JobMagnet.Infrastructure.Repositories.Interfaces;
using JobMagnet.Mappers;
using JobMagnet.Models.Summary;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace JobMagnet.Controllers.V1;

[ApiVersion("1")]
public class SummaryController(
    ILogger<SummaryController> logger,
    ISummaryQueryRepository queryRepository,
    ICommandRepository<SummaryEntity> commandRepository) : BaseController<SummaryController>(logger)
{
    [HttpPost]
    [ProducesResponseType(typeof(SummaryModel), StatusCodes.Status201Created)]
    public async Task<IResult> CreateAsync([FromBody] SummaryCreateRequest createRequest)
    {
        var entity = SummaryMapper.ToEntity(createRequest);
        await commandRepository.CreateAsync(entity).ConfigureAwait(false);
        var newRecord = SummaryMapper.ToModel(entity);

        return Results.CreatedAtRoute(nameof(GetSummaryByIdAsync), new { id = newRecord.Id }, newRecord);
    }

    [HttpGet("{id:long}", Name = nameof(GetSummaryByIdAsync))]
    [ProducesResponseType(typeof(SummaryModel), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IResult> GetSummaryByIdAsync(long id)
    {
        var entity = await queryRepository
            .IncludeEducation()
            .IncludeWorkExperience()
            .GetByIdWithIncludesAsync(id).ConfigureAwait(false);

        if (entity is null)
            return Results.NotFound();

        var responseModel = SummaryMapper.ToModel(entity);

        return Results.Ok(responseModel);
    }

    [HttpDelete("{id:long}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IResult> DeleteAsync(int id)
    {
        var entity = await queryRepository.GetByIdAsync(id).ConfigureAwait(false);

        if (entity is null)
            return Results.NotFound();

        _ = await commandRepository.HardDeleteAsync(entity).ConfigureAwait(false);

        return Results.NoContent();
    }

    [HttpPatch("{id:long}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IResult> PatchAsync(int id, [FromBody] JsonPatchDocument<SummaryRequest> patchDocument)
    {
        _ = queryRepository.IncludeEducation().IncludeWorkExperience();
        var entity = await queryRepository.GetByIdWithIncludesAsync(id).ConfigureAwait(false);

        if (entity is null)
            return Results.NotFound();

        var updateRequest = SummaryMapper.ToUpdateRequest(entity);

        patchDocument.ApplyTo(updateRequest);

        entity.UpdateEntity(updateRequest);

        await commandRepository.UpdateAsync(entity).ConfigureAwait(false);

        return Results.NoContent();
    }

    [HttpPatch("{id:long}/education")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IResult> PatchEducationAsync(int id,
        [FromBody] JsonPatchDocument<SummaryComplexRequest> patchDocument)
    {
        _ = queryRepository.IncludeEducation();
        var entity = await queryRepository.GetByIdWithIncludesAsync(id).ConfigureAwait(false);

        if (entity is null)
            return Results.NotFound();

        var updateRequest = SummaryMapper.ToUpdateComplexRequest(entity);

        patchDocument.ApplyTo(updateRequest);

        entity.UpdateComplexEntity(updateRequest);

        await commandRepository.UpdateAsync(entity).ConfigureAwait(false);

        return Results.NoContent();
    }

    [HttpPatch("{id:long}/WorkExperience")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IResult> PatchWorkExperienceAsync(int id,
        [FromBody] JsonPatchDocument<SummaryComplexRequest> patchDocument)
    {
        _ = queryRepository.IncludeWorkExperience();
        var entity = await queryRepository.GetByIdWithIncludesAsync(id).ConfigureAwait(false);

        if (entity is null)
            return Results.NotFound();

        var updateRequest = SummaryMapper.ToUpdateComplexRequest(entity);

        patchDocument.ApplyTo(updateRequest);

        entity.UpdateComplexEntity(updateRequest);

        await commandRepository.UpdateAsync(entity).ConfigureAwait(false);

        return Results.NoContent();
    }
}