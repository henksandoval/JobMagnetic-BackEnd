using Asp.Versioning;
using JobMagnet.Application.Contracts.Commands.Resume;
using JobMagnet.Application.Contracts.Responses.Resume;
using JobMagnet.Application.Mappers;
using JobMagnet.Domain.Aggregates.Profiles;
using JobMagnet.Domain.Aggregates.Profiles.Entities;
using JobMagnet.Domain.Ports.Repositories.Base;
using JobMagnet.Host.Controllers.Base;
using Microsoft.AspNetCore.Mvc;

namespace JobMagnet.Host.Controllers.V1;

[ApiVersion("1")]
public class ResumeController(
    ILogger<ResumeController> logger,
    IQueryRepository<Headline, long> queryRepository,
    ICommandRepository<Headline> commandRepository) : BaseController<ResumeController>(logger)
{
    [HttpPost]
    [ProducesResponseType(typeof(ResumeResponse), StatusCodes.Status201Created)]
    public async Task<IResult> CreateAsync([FromBody] ResumeCommand createCommand, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
//TODO: Implement the logic to create a new Resume entity from the createCommand.
/*
        var entity = Headline.CreateInstance(new CareerHistoryId(),
            new ProfileId(), createCommand.ResumeData.Title, createCommand.ResumeData.Suffix, createCommand.ResumeData.JobTitle, createCommand.ResumeData.About, createCommand.ResumeData.Summary, createCommand.ResumeData.Overview, createCommand.ResumeData.Address);
        await commandRepository.CreateAsync(entity, cancellationToken);
        await commandRepository.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        var newRecord = entity.ToModel();

        return Results.CreatedAtRoute(nameof(GetResumeByIdAsync), new { id = newRecord.Id }, newRecord);
*/
    }

    [HttpGet("{id:long}", Name = nameof(GetResumeByIdAsync))]
    [ProducesResponseType(typeof(ResumeResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IResult> GetResumeByIdAsync(long id, CancellationToken cancellationToken)
    {
        var entity = await queryRepository.GetByIdAsync(id, cancellationToken);

        if (entity is null)
            return Results.NotFound();

        var responseModel = entity.ToModel();

        return Results.Ok(responseModel);
    }

    [HttpPut("{id:long}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IResult> PutAsync(int id, ResumeCommand command, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();

        var entity = await queryRepository.GetByIdAsync(id, cancellationToken);

        if (entity is null)
            return Results.NotFound();

        var data = command.ResumeData;

        entity.UpdateGeneralInfo(
            data.Title,
            data.Suffix,
            data.JobTitle,
            data.About,
            data.Summary,
            data.Overview,
            data.Address
        );

        await commandRepository
            .Update(entity)
            .SaveChangesAsync(cancellationToken)
            .ConfigureAwait(false);

        return Results.NoContent();
    }

    [HttpDelete("{id:long}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IResult> DeleteAsync(int id, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();

        var entity = await queryRepository.GetByIdAsync(id, cancellationToken).ConfigureAwait(false);

        if (entity is null)
            return Results.NotFound();

        await commandRepository
            .HardDelete(entity)
            .SaveChangesAsync(cancellationToken)
            .ConfigureAwait(false);

        return Results.NoContent();
    }
}