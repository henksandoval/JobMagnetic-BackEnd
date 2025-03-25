using System.Net.Mime;
using JobMagnet.Infrastructure.Entities;
using JobMagnet.Infrastructure.Repositories.Base.Interfaces;
using JobMagnet.Mappers;
using JobMagnet.Models.Resume;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace JobMagnet.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[Consumes(MediaTypeNames.Application.Json)]
public class ResumeController(
    IQueryRepository<ResumeEntity, long> queryRepository,
    ICommandRepository<ResumeEntity> commandRepository) : ControllerBase
{
    [HttpGet("{id:int}", Name = nameof(GetAboutByIdAsync))]
    [ProducesResponseType(typeof(ResumeModel), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IResult> GetAboutByIdAsync(long id)
    {
        var entity = await queryRepository.GetByIdAsync(id);

        if (entity is null)
            return Results.NotFound();

        var responseModel = PersonalInfoMapper.ToModel(entity);

        return Results.Ok(responseModel);
    }

    [HttpPost]
    [ProducesResponseType(typeof(ResumeModel), StatusCodes.Status201Created)]
    public async Task<IResult> CreateAsync([FromBody] ResumeCreateRequest createRequest)
    {
        var entity = PersonalInfoMapper.ToEntity(createRequest);
        await commandRepository.CreateAsync(entity);
        var newRecord = PersonalInfoMapper.ToModel(entity);

        return Results.CreatedAtRoute(nameof(GetAboutByIdAsync), new { id = newRecord.Id }, newRecord);
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IResult> DeleteAsync(int id)
    {
        var entity = await queryRepository.GetByIdAsync(id);

        if (entity is null)
            return Results.NotFound();

        _ = await commandRepository.HardDeleteAsync(entity);

        return Results.NoContent();
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IResult> PutAsync(int id, ResumeUpdateRequest updateRequest)
    {
        if (id != updateRequest.Id)
            return Results.BadRequest();

        var entity = await queryRepository.GetByIdAsync(id);

        if (entity is null)
            return Results.NotFound();

        entity.UpdateEntity(updateRequest);

        await commandRepository.UpdateAsync(entity);

        return Results.NoContent();
    }

    [HttpPatch("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IResult> PatchAsync(int id, [FromBody] JsonPatchDocument<ResumeUpdateRequest> patchDocument)
    {
        var entity = await queryRepository.GetByIdAsync(id);

        if (entity is null)
            return Results.NotFound();

        var updateRequest = PersonalInfoMapper.ToUpdateRequest(entity);

        patchDocument.ApplyTo(updateRequest);

        entity.UpdateEntity(updateRequest);

        await commandRepository.UpdateAsync(entity);

        return Results.NoContent();
    }
}