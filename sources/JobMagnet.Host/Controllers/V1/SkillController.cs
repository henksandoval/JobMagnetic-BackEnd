using Asp.Versioning;
using JobMagnet.Application.Contracts.Commands.Skill;
using JobMagnet.Application.Contracts.Responses.Skill;
using JobMagnet.Application.Mappers;
using JobMagnet.Domain.Core.Entities;
using JobMagnet.Domain.Ports.Repositories;
using JobMagnet.Domain.Ports.Repositories.Base;
using JobMagnet.Host.Controllers.Base;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace JobMagnet.Host.Controllers.V1;

[ApiVersion("1")]
public class SkillController(
    ILogger<SkillController> logger,
    ISkillQueryRepository queryRepository,
    ICommandRepository<SkillEntity> commandRepository) : BaseController<SkillController>(logger)
{
    [HttpPost]
    [ProducesResponseType(typeof(SkillResponse), StatusCodes.Status201Created)]
    public async Task<IResult> CreateAsync([FromBody] SkillCommand createCommand, CancellationToken cancellationToken)
    {
        var entity = createCommand.ToEntity();
        await commandRepository.CreateAsync(entity, cancellationToken).ConfigureAwait(false);
        await commandRepository.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        var newRecord = entity.ToModel();

        return Results.CreatedAtRoute(nameof(GetSkillByIdAsync), new { id = newRecord.Id }, newRecord);
    }

    [HttpGet("{id:long}", Name = nameof(GetSkillByIdAsync))]
    [ProducesResponseType(typeof(SkillResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IResult> GetSkillByIdAsync(long id)
    {
        var entity = await queryRepository
            .IncludeDetails()
            .GetByIdWithIncludesAsync(id).ConfigureAwait(false);

        if (entity is null)
            return Results.NotFound();

        var responseModel = entity.ToModel();

        return Results.Ok(responseModel);
    }

    [HttpDelete("{id:long}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IResult> DeleteAsync(int id, CancellationToken cancellationToken)
    {
        var entity = await queryRepository.GetByIdAsync(id).ConfigureAwait(false);

        if (entity is null)
            return Results.NotFound();

        await commandRepository
            .HardDelete(entity)
            .SaveChangesAsync(cancellationToken)
            .ConfigureAwait(false);

        return Results.NoContent();
    }

    [HttpPatch("{id:long}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IResult> PatchAsync(int id, [FromBody] JsonPatchDocument<SkillCommand> patchDocument, CancellationToken cancellationToken)
    {
        _ = queryRepository.IncludeDetails();
        var entity = await queryRepository.GetByIdWithIncludesAsync(id).ConfigureAwait(false);

        if (entity is null)
            return Results.NotFound();

        var updateRequest = entity.ToUpdateCommand();

        patchDocument.ApplyTo(updateRequest);

        entity.UpdateEntity(updateRequest);

        await commandRepository
            .Update(entity)
            .SaveChangesAsync(cancellationToken)
            .ConfigureAwait(false);

        return Results.NoContent();
    }
}