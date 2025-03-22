using System.Net.Mime;
using JobMagnet.Entities;
using JobMagnet.Mappers;
using JobMagnet.Models;
using JobMagnet.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace JobMagnet.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[Consumes(MediaTypeNames.Application.Json)]
public class AboutController(
    IQueryRepository<AboutEntity> queryRepository,
    ICommandRepository<AboutEntity> commandRepository) : ControllerBase
{
    [HttpGet("{id:int}", Name = nameof(GetAboutByIdAsync))]
    [ProducesResponseType(typeof(AboutModel), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IResult> GetAboutByIdAsync(int id)
    {
        var entity = await queryRepository.GetByIdAsync(id);

        if (entity is null)
            return Results.NotFound($"Record [{id}] not found");

        var responseModel = AboutMapper.ToModel(entity);

        return Results.Ok(responseModel);
    }

    [HttpPost]
    [ProducesResponseType(typeof(AboutModel), StatusCodes.Status201Created)]
    public async Task<IResult> Create([FromBody] AboutCreateRequest createRequest)
    {
        var entity = AboutMapper.ToEntity(createRequest);
        await commandRepository.CreateAsync(entity);
        var newRecord = AboutMapper.ToModel(entity);

        return Results.CreatedAtRoute(nameof(GetAboutByIdAsync), new { id = newRecord.Id }, newRecord);
    }
}