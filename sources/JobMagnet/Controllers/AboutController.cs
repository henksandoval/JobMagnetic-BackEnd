using System.Net.Mime;
using JobMagnet.AutoMapper;
using JobMagnet.Entities;
using JobMagnet.Models;
using JobMagnet.Repositories.Interface;
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

        var responseModel = Mappers.MapAboutModel(entity);

        return Results.Ok(responseModel);
    }

    [HttpPost]
    [ProducesResponseType(typeof(AboutModel), StatusCodes.Status201Created)]
    public async Task<IResult> Create([FromBody] AboutCreateRequest createRequest)
    {
        var entity = Mappers.MapAboutCreate(createRequest);
        await commandRepository.CreateAsync(entity);
        var newRecord = Mappers.MapAboutModel(entity);

        return Results.CreatedAtRoute(nameof(GetAboutByIdAsync), new { id = newRecord.Id }, newRecord);
    }
}