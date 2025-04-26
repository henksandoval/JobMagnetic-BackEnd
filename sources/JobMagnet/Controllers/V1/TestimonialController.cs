using Asp.Versioning;
using JobMagnet.Controllers.Base;
using JobMagnet.Infrastructure.Entities;
using JobMagnet.Infrastructure.Repositories.Base.Interfaces;
using JobMagnet.Mappers;
using JobMagnet.Models.Commands.Testimonial;
using JobMagnet.Models.Responses.Testimonial;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace JobMagnet.Controllers.V1;

[ApiVersion("1")]
public class TestimonialController(
    ILogger<TestimonialController> logger,
    IQueryRepository<TestimonialEntity, long> queryRepository,
    ICommandRepository<TestimonialEntity> commandRepository) : BaseController<TestimonialController>(logger)
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
    public async Task<IResult> CreateAsync([FromBody] TestimonialCreateCommand createCommand)
    {
        var entity = TestimonialMapper.ToEntity(createCommand);
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
    public async Task<IResult> PutAsync(int id, TestimonialUpdateCommand updateCommand)
    {
        if (id != updateCommand.Id)
            return Results.BadRequest();

        var entity = await queryRepository.GetByIdAsync(id).ConfigureAwait(false);

        if (entity is null)
            return Results.NotFound();

        entity.UpdateEntity(updateCommand);

        await commandRepository.UpdateAsync(entity);

        return Results.NoContent();
    }

    [HttpPatch("{id:long}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IResult> PatchAsync(int id, [FromBody] JsonPatchDocument<TestimonialUpdateCommand> patchDocument)
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