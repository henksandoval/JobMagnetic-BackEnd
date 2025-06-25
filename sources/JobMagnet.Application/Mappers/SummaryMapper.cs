using JobMagnet.Application.Contracts.Commands.Summary;
using JobMagnet.Application.Contracts.Responses.Base;
using JobMagnet.Application.Contracts.Responses.Summary;
using JobMagnet.Domain.Core.Entities;
using Mapster;

namespace JobMagnet.Application.Mappers;

public static class SummaryMapper
{
    static SummaryMapper()
    {
        TypeAdapterConfig<SummaryEntity, SummaryResponse>
            .NewConfig()
            .Map(dest => dest.SummaryData, src => src);

        TypeAdapterConfig<SummaryCommand, SummaryEntity>
            .NewConfig()
            .Map(dest => dest, src => src.SummaryData)
            .Ignore(dest => dest.Id);

        TypeAdapterConfig<SummaryCommand, SummaryEntity>
            .NewConfig()
            .Map(dest => dest, src => src.SummaryData);

        TypeAdapterConfig<SummaryEntity, SummaryCommand>
            .NewConfig()
            .Map(dest => dest.SummaryData, src => src);

        TypeAdapterConfig<EducationBase, EducationEntity>
            .NewConfig()
            .MapToConstructor(true);

        TypeAdapterConfig<WorkExperienceBase, WorkExperienceEntity>
            .NewConfig()
            .MapToConstructor(true);
    }

    public static SummaryEntity ToEntity(this SummaryCommand command)
    {
        var data = command.SummaryData;
        var entity = new SummaryEntity(data!.Introduction!, data.ProfileId);

        foreach (var educationBase in data.Education)
        {
            var education = educationBase.Adapt<EducationEntity>();
            entity.AddEducation(education);
        }

        foreach (var experienceBase in data.WorkExperiences)
        {
            var workExperience = experienceBase.Adapt<WorkExperienceEntity>();

            foreach (var description in experienceBase?.Responsibilities ?? [])
                workExperience.AddResponsibility(new WorkResponsibilityEntity(description));

            entity.AddWorkExperience(workExperience);
        }

        ;

        return entity;
    }

    public static SummaryResponse ToModel(this SummaryEntity entity) => entity.Adapt<SummaryResponse>();

    public static SummaryCommand ToUpdateCommand(this SummaryEntity entity) => entity.Adapt<SummaryCommand>();

    public static void UpdateEntity(this SummaryEntity entity, SummaryCommand command)
    {
        command.Adapt(entity);
    }
}