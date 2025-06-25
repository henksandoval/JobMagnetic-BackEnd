using JobMagnet.Application.Contracts.Commands.Summary;
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

        entity.Education = data.Education.Select(e => new EducationEntity(
                e.Degree ?? string.Empty,
                e.InstitutionName ?? string.Empty,
                e.InstitutionLocation ?? string.Empty,
                e.StartDate,
                e.EndDate,
                e.Description ?? string.Empty,
                entity.Id
            )).ToList();

        entity.WorkExperiences = data.WorkExperiences.Select(w => new WorkExperienceEntity
        {
            Id = w.Id,
            Description = w.Description!,
            JobTitle = w.JobTitle!,
            CompanyName = w.CompanyName!,
            CompanyLocation = w.CompanyLocation!,
            StartDate = w.StartDate ?? DateTime.MinValue,
            EndDate = w.EndDate,
            SummaryId = entity.Id,
            Responsibilities = w.Responsibilities.Select(description => new WorkResponsibilityEntity(
                w.Id,
                description
            )).ToList()
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