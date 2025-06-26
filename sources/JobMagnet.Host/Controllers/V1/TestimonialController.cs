using Asp.Versioning;
using JobMagnet.Application.Contracts.Commands.Testimonial;
using JobMagnet.Application.Contracts.Responses.Testimonial;
using JobMagnet.Application.Mappers;
using JobMagnet.Domain.Core.Entities;
using JobMagnet.Domain.Ports.Repositories.Base;
using JobMagnet.Host.Controllers.Base;
using Microsoft.AspNetCore.Mvc;

namespace JobMagnet.Host.Controllers.V1;

[ApiVersion("1")]
public class TestimonialController(
    ILogger<TestimonialController> logger,
    IQueryRepository<Testimonial, long> queryRepository,
    ICommandRepository<Testimonial> commandRepository) : BaseController<TestimonialController>(logger)
{
    [HttpGet("{id:long}", Name = nameof(GetTestimonialByIdAsync))]
    [ProducesResponseType(typeof(TestimonialResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IResult> GetTestimonialByIdAsync(long id, CancellationToken cancellationToken)
    {
        var entity = await queryRepository.GetByIdAsync(id, cancellationToken).ConfigureAwait(false);

        if (entity is null)
            return Results.NotFound();

        var responseModel = entity.ToModel();

        return Results.Ok(responseModel);
    }

    [HttpPost]
    [ProducesResponseType(typeof(TestimonialResponse), StatusCodes.Status201Created)]
    public async Task<IResult> CreateAsync([FromBody] TestimonialCommand createCommand, CancellationToken cancellationToken)
    {
        var entity = createCommand.ToEntity();
        await commandRepository.CreateAsync(entity, cancellationToken).ConfigureAwait(false);
        await commandRepository.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        var newRecord = entity.ToModel();

        return Results.CreatedAtRoute(nameof(GetTestimonialByIdAsync), new { id = newRecord.Id }, newRecord);
    }

    [HttpDelete("{id:long}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IResult> DeleteAsync(int id, CancellationToken cancellationToken)
    {
        var entity = await queryRepository.GetByIdAsync(id, cancellationToken).ConfigureAwait(false);

        if (entity is null)
            return Results.NotFound();

        await commandRepository
            .HardDelete(entity)
            .SaveChangesAsync(cancellationToken)
            .ConfigureAwait(false);

        return Results.NoContent();
    }

    [HttpPut("{id:long}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IResult> PutAsync(int id, TestimonialCommand command, CancellationToken cancellationToken)
    {
        var entity = await queryRepository.GetByIdAsync(id, cancellationToken).ConfigureAwait(false);

        if (entity is null)
            return Results.NotFound();

        entity.UpdateEntity(command);

        await commandRepository
            .Update(entity)
            .SaveChangesAsync(cancellationToken)
            .ConfigureAwait(false);

        return Results.NoContent();
    }
}