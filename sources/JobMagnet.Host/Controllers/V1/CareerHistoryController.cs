using Asp.Versioning;
using JobMagnet.Application.Contracts.Commands.Summary;
using JobMagnet.Application.Contracts.Responses.Summary;
using JobMagnet.Application.Mappers;
using JobMagnet.Domain.Aggregates.Profiles.Entities;
using JobMagnet.Domain.Ports.Repositories;
using JobMagnet.Domain.Ports.Repositories.Base;
using JobMagnet.Host.Controllers.Base;
using Microsoft.AspNetCore.Mvc;

namespace JobMagnet.Host.Controllers.V1;

[ApiVersion("1")]
public class CareerHistoryController(
    ILogger<CareerHistoryController> logger,
    ISummaryQueryRepository queryRepository,
    ICommandRepository<CareerHistory> commandRepository) : BaseController<CareerHistoryController>(logger)
{
    [HttpPost]
    [ProducesResponseType(typeof(SummaryResponse), StatusCodes.Status201Created)]
    public async Task<IResult> CreateAsync([FromBody] SummaryCommand createCommand, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
//TODO: Implement the logic to create a new CareerHistory entity from the createCommand.
/*
        var entity = createCommand.ToEntity();
        await commandRepository.CreateAsync(entity, cancellationToken).ConfigureAwait(false);
        await commandRepository.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        var newRecord = entity.ToModel();

        return Results.CreatedAtRoute(nameof(GetSummaryByIdAsync), new { id = newRecord.Id }, newRecord);
*/
    }

    [HttpGet("{id:guid}", Name = nameof(GetSummaryByIdAsync))]
    [ProducesResponseType(typeof(SummaryResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IResult> GetSummaryByIdAsync(Guid id)
    {
        var entity = await queryRepository
            .IncludeEducation()
            .IncludeWorkExperience()
            .GetByIdWithIncludesAsync(new CareerHistoryId(id)).ConfigureAwait(false);

        if (entity is null)
            return Results.NotFound();

        var responseModel = entity.ToModel();

        return Results.Ok(responseModel);
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IResult> DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();

        var entity = await queryRepository.GetByIdAsync(new CareerHistoryId(id), cancellationToken).ConfigureAwait(false);

        if (entity is null)
            return Results.NotFound();

        await commandRepository
            .HardDelete(entity)
            .SaveChangesAsync(cancellationToken)
            .ConfigureAwait(false);

        return Results.NoContent();
    }
}