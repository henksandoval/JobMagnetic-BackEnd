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
        var entity = new SummaryEntity
        {
            Id = 0,
            Introduction = data!.Introduction!,
            ProfileId = data.ProfileId,
        };

        entity.Education = data.Education.Select(e => e.Adapt<EducationEntity>()).ToList();

        entity.WorkExperiences = data.WorkExperiences.Select(w =>
        {
            var workExperience = w.Adapt<WorkExperienceEntity>();
            if (w.Responsibilities == null) return workExperience;

            foreach (var description in w.Responsibilities)
                workExperience.AddResponsibility(new WorkResponsibilityEntity(description));

            return workExperience;
        }).ToList();

        return entity;
    }

    public static SummaryResponse ToModel(this SummaryEntity entity)
    {
        return entity.Adapt<SummaryResponse>();
    }

    public static SummaryCommand ToUpdateCommand(this SummaryEntity entity)
    {
        return entity.Adapt<SummaryCommand>();
    }

    public static void UpdateEntity(this SummaryEntity entity, SummaryCommand command)
    {
        command.Adapt(entity);
    }
}