using Asp.Versioning;
using JobMagnet.Application.Contracts.Commands.Resume;
using JobMagnet.Application.Contracts.Responses.Resume;
using JobMagnet.Application.Mappers;
using JobMagnet.Domain.Core.Entities;
using JobMagnet.Domain.Ports.Repositories.Base;
using JobMagnet.Host.Controllers.Base;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace JobMagnet.Host.Controllers.V1;

[ApiVersion("1")]
public class ResumeController(
    ILogger<ResumeController> logger,
    IQueryRepository<ResumeEntity, long> queryRepository,
    ICommandRepository<ResumeEntity> commandRepository) : BaseController<ResumeController>(logger)
{
    [HttpPost]
    [ProducesResponseType(typeof(ResumeResponse), StatusCodes.Status201Created)]
    public async Task<IResult> CreateAsync([FromBody] ResumeCommand createCommand, CancellationToken cancellationToken)
    {
        var entity = createCommand.ToEntity();
        await commandRepository.CreateAsync(entity, cancellationToken);
        await commandRepository.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        var newRecord = entity.ToModel();

        return Results.CreatedAtRoute(nameof(GetResumeByIdAsync), new { id = newRecord.Id }, newRecord);
    }

    [HttpGet("{id:long}", Name = nameof(GetResumeByIdAsync))]
    [ProducesResponseType(typeof(ResumeResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IResult> GetResumeByIdAsync(long id)
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
    public async Task<IResult> PutAsync(int id, ResumeCommand command, CancellationToken cancellationToken)
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
    public async Task<IResult> PatchAsync(int id, [FromBody] JsonPatchDocument<ResumeCommand> patchDocument, CancellationToken cancellationToken)
    {
        var entity = await queryRepository.GetByIdAsync(id).ConfigureAwait(false);

        if (entity is null)
            return Results.NotFound();

        var updateRequest = entity.ToUpdateRequest();

        patchDocument.ApplyTo(updateRequest);

        entity.UpdateEntity(updateRequest);

        await commandRepository
            .Update(entity)
            .SaveChangesAsync(cancellationToken)
            .ConfigureAwait(false);

        return Results.NoContent();
    }
}