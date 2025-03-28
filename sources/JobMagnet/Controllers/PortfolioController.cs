using System.Net.Mime;
using JobMagnet.Infrastructure.Entities;
using JobMagnet.Infrastructure.Repositories.Base.Interfaces;
using JobMagnet.Infrastructure.Repositories.Interfaces;
using JobMagnet.Mappers;
using JobMagnet.Models.Portfolio;
using JobMagnet.Models.Resume;
using Microsoft.AspNetCore.Mvc;

namespace JobMagnet.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[Consumes(MediaTypeNames.Application.Json)]
public class PortfolioController(
    IPortfolioQueryRepository queryRepository,
    ICommandRepository<PortfolioEntity> commandRepository) : ControllerBase
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

    [HttpGet("{id:int}", Name = nameof(GetPortfolioByIdAsync))]
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
}