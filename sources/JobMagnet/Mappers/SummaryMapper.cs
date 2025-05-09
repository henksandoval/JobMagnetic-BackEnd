using JobMagnet.Infrastructure.Entities;
using JobMagnet.Models.Commands.Summary;
using JobMagnet.Models.Responses.Summary;
using Mapster;

namespace JobMagnet.Mappers;

internal static class SummaryMapper
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

    internal static SummaryEntity ToEntity(this SummaryCommand command)
    {
        return command.Adapt<SummaryEntity>();
    }

    internal static SummaryModel ToModel(this SummaryEntity entity)
    {
        return entity.Adapt<SummaryModel>();
    }

    internal static SummaryCommand ToUpdateCommand(this SummaryEntity entity)
    {
        return entity.Adapt<SummaryCommand>();
    }

    internal static void UpdateEntity(this SummaryEntity entity, SummaryCommand command)
    {
        command.Adapt(entity);
    }
}