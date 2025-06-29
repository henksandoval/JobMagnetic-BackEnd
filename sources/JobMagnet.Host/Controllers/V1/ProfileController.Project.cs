using JobMagnet.Application.Contracts.Commands.Portfolio;
using JobMagnet.Application.Contracts.Responses.Portfolio;
using JobMagnet.Application.Mappers;
using JobMagnet.Domain.Aggregates.Profiles;
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

        var profile = await queryRepository
            .WhereCondition(p => p.Id == new ProfileId(profileId))
            .WithProject()
            .BuildFirstOrDefaultAsync(cancellationToken)
            .ConfigureAwait(false);

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
        var profile = await queryRepository
            .WhereCondition(p => p.Id == new ProfileId(profileId))
            .WithProject()
            .BuildFirstOrDefaultAsync(cancellationToken)
            .ConfigureAwait(false);

        if (profile is null)
        {
            return Results.NotFound("Profile not found.");
        }

        var response = profile.Projects
            .Select(project => project.ToModel())
            .ToList();

        return Results.Ok(response);
    }
}