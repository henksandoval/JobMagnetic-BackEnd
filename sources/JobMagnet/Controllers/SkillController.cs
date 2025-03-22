using System.Net.Mime;
using JobMagnet.AutoMapper;
using JobMagnet.Entities;
using JobMagnet.Models;
using JobMagnet.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace JobMagnet.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[Consumes(MediaTypeNames.Application.Json)]
public class SkillController(ICommandRepository<SkillEntity> commandRepository) : ControllerBase
{
    [HttpGet("{id:int}", Name = nameof(GetSkillByIdAsync))]
    public async Task<IResult> GetSkillByIdAsync(int id)
    {
        await Task.Delay(1);
        throw new NotImplementedException();
    }

    [HttpPost]
    [ProducesResponseType(typeof(SkillModel), StatusCodes.Status201Created)]
    public async Task<IResult> CreateAsync([FromBody] SkillCreateRequest createRequest)
    {
        var entity = Mappers.MapSkillCreate(createRequest);
        await commandRepository.CreateAsync(entity);
        var newRecord = Mappers.MapSkillModel(entity);

        return Results.CreatedAtRoute(nameof(GetSkillByIdAsync), new { id = newRecord.Id }, newRecord);
    }
}