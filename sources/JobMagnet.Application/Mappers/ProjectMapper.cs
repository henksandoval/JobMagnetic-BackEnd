using JobMagnet.Application.Contracts.Commands.Portfolio;
using JobMagnet.Application.Contracts.Responses.Portfolio;
using JobMagnet.Domain.Aggregates.Profiles.Entities;
using Mapster;

namespace JobMagnet.Application.Mappers;

public static class ProjectMapper
{
    static ProjectMapper()
    {
        TypeAdapterConfig<Project, ProjectResponse>
            .NewConfig()
            .Map(dest => dest.ProjectData, src => src);

        TypeAdapterConfig<Project, ProjectCommand>
            .NewConfig()
            .Map(dest => dest.ProjectData, src => src);
    }

    public static ProjectResponse ToModel(this Project project) => project.Adapt<ProjectResponse>();

    public static ProjectCommand ToUpdateRequest(this Project project) => project.Adapt<ProjectCommand>();
}