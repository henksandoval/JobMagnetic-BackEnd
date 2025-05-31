using Asp.Versioning;
using JobMagnet.Application.Contracts.Commands.Service;
using JobMagnet.Application.Contracts.Responses.Service;
using JobMagnet.Application.Mappers;
using JobMagnet.Domain.Core.Entities;
using JobMagnet.Domain.Ports.Repositories;
using JobMagnet.Domain.Ports.Repositories.Base;
using JobMagnet.Host.Controllers.Base;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace JobMagnet.Host.Controllers.V1;

[ApiVersion("1")]
public class ServiceController(
    ILogger<ServiceController> logger,
    IServiceQueryRepository queryRepository,
    ICommandRepository<ServiceEntity> commandRepository) : BaseController<ServiceController>(logger)
{
    [HttpPost]
    [ProducesResponseType(typeof(ServiceModel), StatusCodes.Status201Created)]
    public async Task<IResult> CreateAsync([FromBody] ServiceCommand createCommand)
    {
        var entity = createCommand.ToEntity();
        await commandRepository.CreateAsync(entity).ConfigureAwait(false);
        var newRecord = entity.ToModel();

        return Results.CreatedAtRoute(nameof(GetServiceByIdAsync), new { id = newRecord.Id }, newRecord);
    }

    [HttpGet("{id:long}", Name = nameof(GetServiceByIdAsync))]
    [ProducesResponseType(typeof(ServiceModel), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IResult> GetServiceByIdAsync(long id)
    {
        var entity = await queryRepository
            .IncludeGalleryItems()
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

        _ = await commandRepository.HardDeleteAsync(entity).ConfigureAwait(false);

        return Results.NoContent();
    }

    [HttpPatch("{id:long}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IResult> PatchAsync(int id, [FromBody] JsonPatchDocument<ServiceCommand> patchDocument)
    {
        _ = queryRepository.IncludeGalleryItems();
        var entity = await queryRepository.GetByIdWithIncludesAsync(id).ConfigureAwait(false);

        if (entity is null)
            return Results.NotFound();

        var updateRequest = entity.ToUpdateCommand();

        patchDocument.ApplyTo(updateRequest);

        entity.UpdateEntity(updateRequest);

        await commandRepository.UpdateAsync(entity).ConfigureAwait(false);

        return Results.NoContent();
    }
}