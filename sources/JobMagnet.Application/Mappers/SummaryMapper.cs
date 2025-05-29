using JobMagnet.Application.Commands.Summary;
using JobMagnet.Application.Models.Responses.Summary;
using JobMagnet.Domain.Entities;
using Mapster;

namespace JobMagnet.Application.Mappers;

public static class SummaryMapper
{
    static SummaryMapper()
    {
        TypeAdapterConfig<SummaryEntity, SummaryModel>
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
        return command.Adapt<SummaryEntity>();
    }

    public static SummaryModel ToModel(this SummaryEntity entity)
    {
        return entity.Adapt<SummaryModel>();
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