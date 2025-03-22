using System.Net.Mime;
using JobMagnet.AutoMapper;
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
public class SkillController(
    IQueryRepository<SkillEntity> queryRepository,
    ICommandRepository<SkillEntity> commandRepository) : ControllerBase
{
    [HttpGet("{id:int}", Name = nameof(GetSkillByIdAsync))]
    public async Task<IResult> GetSkillByIdAsync(int id)
    {
        var entity = await queryRepository.GetByIdAsync(id);

        if (entity is null)
            return Results.NotFound($"Record [{id}] not found");

        var responseModel = SkillMapper.ToModel(entity);

        return Results.Ok(responseModel);
    }

    [HttpPost]
    [ProducesResponseType(typeof(SkillModel), StatusCodes.Status201Created)]
    public async Task<IResult> CreateAsync([FromBody] SkillCreateRequest createRequest)
    {
        var entity = SkillMapper.ToEntity(createRequest);
        await commandRepository.CreateAsync(entity);
        var newRecord = SkillMapper.ToModel(entity);

        return Results.CreatedAtRoute(nameof(GetSkillByIdAsync), new { id = newRecord.Id }, newRecord);
    }
}