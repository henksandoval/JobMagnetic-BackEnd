using Asp.Versioning;
using JobMagnet.Application.Contracts.Commands.Project;
using JobMagnet.Application.Contracts.Responses.Project;
using JobMagnet.Application.Contracts.Responses.Resume;
using JobMagnet.Application.Mappers;
using JobMagnet.Domain.Core.Entities;
using JobMagnet.Domain.Ports.Repositories.Base;
using JobMagnet.Host.Controllers.Base;
using Microsoft.AspNetCore.Mvc;

namespace JobMagnet.Host.Controllers.V1;

[ApiVersion("1")]
public class ProjectController(
    ILogger<ProjectController> logger,
    IQueryRepository<Project, long> queryRepository,
    ICommandRepository<Project> commandRepository) : BaseController<ProjectController>(logger)
{
    [HttpPost]
    [ProducesResponseType(typeof(ProjectResponse), StatusCodes.Status201Created)]
    public async Task<IResult> CreateAsync([FromBody] ProjectCommand command, CancellationToken cancellationToken)
    {
        var entity = command.ToEntity();
        await commandRepository.CreateAsync(entity, cancellationToken).ConfigureAwait(false);
        await commandRepository.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        var newRecord = entity.ToModel();

        return Results.CreatedAtRoute(nameof(GetProjectByIdAsync), new { id = newRecord.Id }, newRecord);
    }

    [HttpGet("{id:long}", Name = nameof(GetProjectByIdAsync))]
    [ProducesResponseType(typeof(ResumeResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IResult> GetProjectByIdAsync(long id, CancellationToken cancellationToken)
    {
        var entity = await queryRepository
            .GetByIdAsync(id, cancellationToken).ConfigureAwait(false);

        if (entity is null)
            return Results.NotFound();

        var responseModel = entity.ToModel();

        return Results.Ok(responseModel);
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
    public async Task<IResult> PutAsync(int id, ProjectCommand updateCommand, CancellationToken cancellationToken)
    {
        var entity = await queryRepository.GetByIdAsync(id, cancellationToken).ConfigureAwait(false);

        if (entity is null)
            return Results.NotFound();

        entity.UpdateEntity(updateCommand);

        await commandRepository
            .Update(entity)
            .SaveChangesAsync(cancellationToken)
            .ConfigureAwait(false);

        return Results.NoContent();
    }
}