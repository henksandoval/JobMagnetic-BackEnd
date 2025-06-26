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
        TypeAdapterConfig<CareerHistory, SummaryResponse>
            .NewConfig()
            .Map(dest => dest.SummaryData, src => src);

        TypeAdapterConfig<SummaryCommand, CareerHistory>
            .NewConfig()
            .Map(dest => dest, src => src.SummaryData)
            .Ignore(dest => dest.Id);

        TypeAdapterConfig<SummaryCommand, CareerHistory>
            .NewConfig()
            .Map(dest => dest, src => src.SummaryData);

        TypeAdapterConfig<CareerHistory, SummaryCommand>
            .NewConfig()
            .Map(dest => dest.SummaryData, src => src);

        TypeAdapterConfig<EducationBase, Qualification>
            .NewConfig()
            .MapToConstructor(true);

        TypeAdapterConfig<WorkExperienceBase, WorkExperience>
            .NewConfig()
            .MapToConstructor(true);
    }

    public static CareerHistory ToEntity(this SummaryCommand command)
    {
        var data = command.SummaryData;
        var entity = new CareerHistory(data!.Introduction!, data.ProfileId);

        foreach (var educationBase in data.Education)
        {
            var education = educationBase.Adapt<Qualification>();
            entity.AddEducation(education);
        }

        foreach (var experienceBase in data.WorkExperiences)
        {
            var workExperience = experienceBase.Adapt<WorkExperience>();

            foreach (var description in experienceBase?.Responsibilities ?? [])
                workExperience.AddResponsibility(new WorkHighlight(description));

            entity.AddWorkExperience(workExperience);
        }

        ;

        return entity;
    }

    public static SummaryResponse ToModel(this CareerHistory entity) => entity.Adapt<SummaryResponse>();

    public static SummaryCommand ToUpdateCommand(this CareerHistory entity) => entity.Adapt<SummaryCommand>();

    public static void UpdateEntity(this CareerHistory entity, SummaryCommand command)
    {
        command.Adapt(entity);
    }
}