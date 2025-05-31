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
        return command.Adapt<SummaryEntity>();
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