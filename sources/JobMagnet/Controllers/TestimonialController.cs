using System.Net.Mime;
using JobMagnet.Infrastructure.Entities;
using JobMagnet.Infrastructure.Repositories.Base.Interfaces;
using JobMagnet.Mappers;
using JobMagnet.Models.Testimonial;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace JobMagnet.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[Consumes(MediaTypeNames.Application.Json)]
public class TestimonialController(
    IQueryRepository<TestimonialEntity, long> queryRepository,
    ICommandRepository<TestimonialEntity> commandRepository) : ControllerBase
{
    [HttpGet("{id:long}", Name = nameof(GetTestimonialByIdAsync))]
    [ProducesResponseType(typeof(TestimonialModel), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IResult> GetTestimonialByIdAsync(long id)
    {
        var entity = await queryRepository.GetByIdAsync(id).ConfigureAwait(false);

        if (entity is null)
            return Results.NotFound();

        var responseModel = TestimonialMapper.ToModel(entity);

        return Results.Ok(responseModel);
    }

    [HttpPost]
    [ProducesResponseType(typeof(TestimonialModel), StatusCodes.Status201Created)]
    public async Task<IResult> CreateAsync([FromBody] TestimonialCreateRequest createRequest)
    {
        var entity = TestimonialMapper.ToEntity(createRequest);
        await commandRepository.CreateAsync(entity).ConfigureAwait(false);
        var newRecord = TestimonialMapper.ToModel(entity);

        return Results.CreatedAtRoute(nameof(GetTestimonialByIdAsync), new { id = newRecord.Id }, newRecord);
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

    [HttpPut("{id:long}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IResult> PutAsync(int id, TestimonialUpdateRequest updateRequest)
    {
        if (id != updateRequest.Id)
            return Results.BadRequest();

        var entity = await queryRepository.GetByIdAsync(id).ConfigureAwait(false);

        if (entity is null)
            return Results.NotFound();

        entity.UpdateEntity(updateRequest);

        await commandRepository.UpdateAsync(entity);

        return Results.NoContent();
    }

    [HttpPatch("{id:long}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IResult> PatchAsync(int id, [FromBody] JsonPatchDocument<TestimonialUpdateRequest> patchDocument)
    {
        var entity = await queryRepository.GetByIdAsync(id).ConfigureAwait(false);

        if (entity is null)
            return Results.NotFound();

        var updateRequest = TestimonialMapper.ToUpdateRequest(entity);

        patchDocument.ApplyTo(updateRequest);

        entity.UpdateEntity(updateRequest);

        await commandRepository.UpdateAsync(entity).ConfigureAwait(false);

        return Results.NoContent();
    }
}