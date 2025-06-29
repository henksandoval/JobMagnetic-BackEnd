using JobMagnet.Application.Contracts.Responses.Base;
using JobMagnet.Application.Contracts.Responses.Portfolio;
using JobMagnet.Domain.Aggregates.Profiles.Entities;
using Mapster;

namespace JobMagnet.Application.Mappers;

public static class ProjectMapper
{
    static ProjectMapper()
    {
        TypeAdapterConfig<Project, ProjectBase>
            .NewConfig()
            .Map(dest => dest.ProfileId, src => src.ProfileId.Value);

        TypeAdapterConfig<Project, ProjectResponse>
            .NewConfig()
            .Map(dest => dest.Id, src => src.Id.Value)
            .Map(dest => dest.ProjectData, src => src);
    }

    public static ProjectResponse ToModel(this Project project) => project.Adapt<ProjectResponse>();
}