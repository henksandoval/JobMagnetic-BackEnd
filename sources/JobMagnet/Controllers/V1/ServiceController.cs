using JobMagnet.Controllers.Base;
using JobMagnet.Infrastructure.Entities;
using JobMagnet.Infrastructure.Repositories.Base.Interfaces;
using JobMagnet.Infrastructure.Repositories.Interfaces;
using JobMagnet.Mappers;
using JobMagnet.Models.Service;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace JobMagnet.Controllers.V1;

public class ServiceController(
    ILogger<ServiceController> logger,
    IServiceQueryRepository queryRepository,
    ICommandRepository<ServiceEntity> commandRepository) : BaseController<ServiceController>(logger)
{
    [HttpPost]
    [ProducesResponseType(typeof(ServiceModel), StatusCodes.Status201Created)]
    public async Task<IResult> CreateAsync([FromBody] ServiceCreateRequest createRequest)
    {
        var entity = ServiceMapper.ToEntity(createRequest);
        await commandRepository.CreateAsync(entity).ConfigureAwait(false);
        var newRecord = ServiceMapper.ToModel(entity);

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

        var responseModel = ServiceMapper.ToModel(entity);

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
    public async Task<IResult> PatchAsync(int id, [FromBody] JsonPatchDocument<ServiceRequest> patchDocument)
    {
        _ = queryRepository.IncludeGalleryItems();
        var entity = await queryRepository.GetByIdWithIncludesAsync(id).ConfigureAwait(false);

        if (entity is null)
            return Results.NotFound();

        var updateRequest = ServiceMapper.ToUpdateRequest(entity);

        patchDocument.ApplyTo(updateRequest);

        entity.UpdateEntity(updateRequest);

        await commandRepository.UpdateAsync(entity).ConfigureAwait(false);

        return Results.NoContent();
    }
}