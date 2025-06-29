using CommunityToolkit.Diagnostics;
using JobMagnet.Application.Contracts.Commands.Portfolio;
using JobMagnet.Application.Contracts.Responses.Portfolio;
using JobMagnet.Application.Mappers;
using JobMagnet.Domain.Aggregates.Profiles;
using JobMagnet.Domain.Aggregates.Profiles.Entities;
using JobMagnet.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace JobMagnet.Host.Controllers.V1;

public partial class ProfileController
{
    [HttpPost("{profileId:guid}/project")]
    [ProducesResponseType(typeof(ProjectResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IResult> AddProjectToProfileAsync(Guid profileId, [FromBody] ProjectCommand command, CancellationToken cancellationToken)
    {
        if (profileId != command.ProjectData?.ProfileId)
            throw new ArgumentException($"{nameof(command.ProjectData.ProfileId)} must be equal to profileId.");

        var profile = await GetProfileWithProjects(profileId, cancellationToken).ConfigureAwait(false);

        if (profile is null)
            return Results.NotFound();

        var project = profile.Portfolio.AddProject(
            guidGenerator,
            clock,
            command.ProjectData.Title ?? string.Empty,
            command.ProjectData.Description ?? string.Empty,
            command.ProjectData.UrlLink ?? string.Empty,
            command.ProjectData.UrlImage ?? string.Empty,
            command.ProjectData.UrlVideo ?? string.Empty,
            command.ProjectData.Type ?? string.Empty
        );

        await projectCommandRepository.CreateAsync(project, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        var result = project.ToModel();

        return Results.CreatedAtRoute("GetProjectsByProfile", new { profileId }, result);
    }

    [HttpGet("{profileId:guid}/projects", Name = "GetProjectsByProfile")]
    [ProducesResponseType(typeof(IEnumerable<ProjectResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IResult> GetProjectsByProfileAsync(Guid profileId, CancellationToken cancellationToken)
    {
        var profile = await GetProfileWithProjects(profileId, cancellationToken);

        if (profile is null)
            return Results.NotFound();

        var response = profile.Projects
            .Select(project => project.ToModel())
            .ToList();

        return Results.Ok(response);
    }

    [HttpPut("{profileId:guid}/projects/{projectId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IResult> UpdateProjectAsync(Guid profileId, Guid projectId, [FromBody] ProjectCommand command,
        CancellationToken cancellationToken)
    {
        var profile = await GetProfileWithProjects(profileId, cancellationToken).ConfigureAwait(false);

        if (profile is null)
            return Results.NotFound();

        var data = command.ProjectData;

        Guard.IsNotNull(data);

        try
        {
            var updatedProject = profile.Portfolio.UpdateProject(
                new ProjectId(projectId),
                data.Title ?? string.Empty,
                data.Description ?? string.Empty,
                data.UrlLink ?? string.Empty,
                data.UrlImage ?? string.Empty,
                data.UrlVideo ?? string.Empty,
                data.Type ?? string.Empty
            );

            projectCommandRepository.Update(updatedProject);
        }
        catch (NotFoundException ex)
        {
            return Results.NotFound(new { ex.Message });
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Results.NoContent();
    }

    [HttpPut("{profileId:guid}/projects/arrange")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IResult> ArrangeProjectsAsync(Guid profileId, [FromBody] List<Guid> orderedProjectIds, CancellationToken cancellationToken)
    {
        try
        {
            var profile = await GetProfileWithProjects(profileId, cancellationToken);

            if (profile is null)
                return Results.NotFound();

            var typedIds = orderedProjectIds.Select(id => new ProjectId(id));

            profile.Portfolio.ArrangeProjects(typedIds);

            projectCommandRepository.UpdateRange(profile.Projects);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            return Results.NoContent();
        }
        catch (NotFoundException ex)
        {
            return Results.NotFound(new { ex.Message });
        }
        catch (BusinessRuleValidationException ex)
        {
            return Results.BadRequest(new { ex.Message });
        }
    }

    [HttpDelete("{profileId:guid}/projects/{projectId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IResult> DeleteProjectAsync(Guid profileId, Guid projectId, CancellationToken cancellationToken)
    {
        var profile = await GetProfileWithProjects(profileId, cancellationToken).ConfigureAwait(false);

        if (profile is null)
            return Results.NotFound();

        try
        {
            var project = profile.Portfolio.RemoveProject(new ProjectId(projectId));
            projectCommandRepository.Remove(project);
        }
        catch (NotFoundException ex)
        {
            return Results.NotFound(new { ex.Message });
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Results.NoContent();
    }

    private async Task<Profile?> GetProfileWithProjects(Guid profileId, CancellationToken cancellationToken)
    {
        return await queryRepository
            .WhereCondition(p => p.Id == new ProfileId(profileId))
            .WithProject()
            .BuildFirstOrDefaultAsync(cancellationToken)
            .ConfigureAwait(false);
    }
}