using JobMagnet.Controllers.Base;
using JobMagnet.Infrastructure.Entities;
using JobMagnet.Infrastructure.Repositories.Base.Interfaces;
using JobMagnet.Infrastructure.Repositories.Interfaces;
using JobMagnet.Mappers;
using JobMagnet.Models.Portfolio;
using JobMagnet.Models.Resume;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace JobMagnet.Controllers.V1;

public class PortfolioController(
    ILogger<PortfolioController> logger,
    IPortfolioQueryRepository queryRepository,
    ICommandRepository<PortfolioGalleryEntity> commandRepository) : BaseController<PortfolioController>(logger)
{
    [HttpPost]
    [ProducesResponseType(typeof(PortfolioModel), StatusCodes.Status201Created)]
    public async Task<IResult> CreateAsync([FromBody] PortfolioCreateRequest createRequest)
    {
        var entity = PortfolioMapper.ToEntity(createRequest);
        await commandRepository.CreateAsync(entity).ConfigureAwait(false);
        var newRecord = PortfolioMapper.ToModel(entity);

        return Results.CreatedAtRoute(nameof(GetPortfolioByIdAsync), new { id = newRecord.Id }, newRecord);
    }

    [HttpGet("{id:long}", Name = nameof(GetPortfolioByIdAsync))]
    [ProducesResponseType(typeof(ResumeModel), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IResult> GetPortfolioByIdAsync(long id)
    {
        var entity = await queryRepository
            .IncludeGalleryItems()
            .GetByIdWithIncludesAsync(id).ConfigureAwait(false);

        if (entity is null)
            return Results.NotFound();

        var responseModel = PortfolioMapper.ToModel(entity);

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
    public async Task<IResult> PatchAsync(int id, [FromBody] JsonPatchDocument<PortfolioRequest> patchDocument)
    {
        _ = queryRepository.IncludeGalleryItems();
        var entity = await queryRepository.GetByIdWithIncludesAsync(id).ConfigureAwait(false);

        if (entity is null)
            return Results.NotFound();

        var updateRequest = PortfolioMapper.ToUpdateRequest(entity);

        patchDocument.ApplyTo(updateRequest);

        entity.UpdateEntity(updateRequest);

        await commandRepository.UpdateAsync(entity).ConfigureAwait(false);

        return Results.NoContent();
    }
}