using Asp.Versioning;
using JobMagnet.Application.Contracts.Commands.Portfolio;
using JobMagnet.Application.Contracts.Responses.Portfolio;
using JobMagnet.Application.Contracts.Responses.Resume;
using JobMagnet.Application.Mappers;
using JobMagnet.Domain.Core.Entities;
using JobMagnet.Domain.Ports.Repositories.Base;
using JobMagnet.Host.Controllers.Base;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace JobMagnet.Host.Controllers.V1;

[ApiVersion("1")]
public class PortfolioController(
    ILogger<PortfolioController> logger,
    IQueryRepository<PortfolioGalleryEntity, long> queryRepository,
    ICommandRepository<PortfolioGalleryEntity> commandRepository) : BaseController<PortfolioController>(logger)
{
    [HttpPost]
    [ProducesResponseType(typeof(PortfolioResponse), StatusCodes.Status201Created)]
    public async Task<IResult> CreateAsync([FromBody] PortfolioCommand command)
    {
        var entity = command.ToEntity();
        await commandRepository.CreateAsync(entity).ConfigureAwait(false);
        await commandRepository.SaveChangesAsync().ConfigureAwait(false);
        var newRecord = entity.ToModel();

        return Results.CreatedAtRoute(nameof(GetPortfolioByIdAsync), new { id = newRecord.Id }, newRecord);
    }

    [HttpGet("{id:long}", Name = nameof(GetPortfolioByIdAsync))]
    [ProducesResponseType(typeof(ResumeResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IResult> GetPortfolioByIdAsync(long id)
    {
        var entity = await queryRepository
            .GetByIdAsync(id).ConfigureAwait(false);

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

    [HttpPut("{id:long}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IResult> PutAsync(int id, PortfolioCommand updateCommand)
    {
        var entity = await queryRepository.GetByIdAsync(id).ConfigureAwait(false);

        if (entity is null)
            return Results.NotFound();

        entity.UpdateEntity(updateCommand);

        commandRepository.Update(entity);
        await commandRepository.SaveChangesAsync().ConfigureAwait(false);

        return Results.NoContent();
    }

    [HttpPatch("{id:long}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IResult> PatchAsync(int id, [FromBody] JsonPatchDocument<PortfolioCommand> patchDocument)
    {
        var entity = await queryRepository.GetByIdAsync(id).ConfigureAwait(false);

        if (entity is null)
            return Results.NotFound();

        var updateRequest = entity.ToUpdateRequest();

        patchDocument.ApplyTo(updateRequest);

        entity.UpdateEntity(updateRequest);

        commandRepository.Update(entity);
        await commandRepository.SaveChangesAsync().ConfigureAwait(false);

        return Results.NoContent();
    }
}