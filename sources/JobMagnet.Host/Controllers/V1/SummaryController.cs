using Asp.Versioning;
using JobMagnet.Application.Contracts.Commands.Summary;
using JobMagnet.Application.Contracts.Responses.Summary;
using JobMagnet.Application.Mappers;
using JobMagnet.Domain.Core.Entities;
using JobMagnet.Domain.Ports.Repositories;
using JobMagnet.Domain.Ports.Repositories.Base;
using JobMagnet.Host.Controllers.Base;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace JobMagnet.Host.Controllers.V1;

[ApiVersion("1")]
public class SummaryController(
    ILogger<SummaryController> logger,
    ISummaryQueryRepository queryRepository,
    ICommandRepository<SummaryEntity> commandRepository) : BaseController<SummaryController>(logger)
{
    [HttpPost]
    [ProducesResponseType(typeof(SummaryResponse), StatusCodes.Status201Created)]
    public async Task<IResult> CreateAsync([FromBody] SummaryCommand createCommand)
    {
        var entity = createCommand.ToEntity();
        await commandRepository.CreateAsync(entity).ConfigureAwait(false);
        await commandRepository.SaveChangesAsync().ConfigureAwait(false);
        var newRecord = entity.ToModel();

        return Results.CreatedAtRoute(nameof(GetSummaryByIdAsync), new { id = newRecord.Id }, newRecord);
    }

    [HttpGet("{id:long}", Name = nameof(GetSummaryByIdAsync))]
    [ProducesResponseType(typeof(SummaryResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IResult> GetSummaryByIdAsync(long id)
    {
        var entity = await queryRepository
            .IncludeEducation()
            .IncludeWorkExperience()
            .GetByIdWithIncludesAsync(id).ConfigureAwait(false);

        if (entity is null)
            return Results.NotFound();

        var responseModel = entity.ToModel();

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

        commandRepository.HardDelete(entity);
        await commandRepository.SaveChangesAsync().ConfigureAwait(false);

        return Results.NoContent();
    }

    [HttpPatch("{id:long}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IResult> PatchAsync(int id, [FromBody] JsonPatchDocument<SummaryCommand> patchDocument)
    {
        _ = queryRepository.IncludeEducation().IncludeWorkExperience();
        var entity = await queryRepository.GetByIdWithIncludesAsync(id).ConfigureAwait(false);

        if (entity is null)
            return Results.NotFound();

        var updateRequest = entity.ToUpdateCommand();

        patchDocument.ApplyTo(updateRequest);

        entity.UpdateEntity(updateRequest);

        commandRepository.Update(entity);
        await commandRepository.SaveChangesAsync().ConfigureAwait(false);

        return Results.NoContent();
    }
}