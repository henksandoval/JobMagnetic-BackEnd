using JobMagnet.Application.Contracts.Commands.Portfolio;
using JobMagnet.Application.Contracts.Responses.Portfolio;
using JobMagnet.Domain.Aggregates.Profiles.Entities;
using JobMagnet.Domain.Core.Entities;
using Mapster;

namespace JobMagnet.Application.Mappers;

public static class ProjectMapper
{
    static ProjectMapper()
    {
        TypeAdapterConfig<Project, ProjectResponse>
            .NewConfig()
            .Map(dest => dest.ProjectData, src => src);

        TypeAdapterConfig<ProjectCommand, Project>
            .NewConfig()
            .Map(dest => dest, src => src.ProjectData)
            .MapToConstructor(true);

        TypeAdapterConfig<Project, ProjectCommand>
            .NewConfig()
            .Map(dest => dest.ProjectData, src => src);
    }

    public static Project ToEntity(this ProjectCommand command) => command.Adapt<Project>();

    public static ProjectResponse ToModel(this Project project) => project.Adapt<ProjectResponse>();

    public static ProjectCommand ToUpdateRequest(this Project project) => project.Adapt<ProjectCommand>();

    public static void UpdateEntity(this Project project, ProjectCommand updateCommand)
    {
        updateCommand.Adapt(project);
    }
}