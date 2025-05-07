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

        TypeAdapterConfig<SummaryCreateCommand, SummaryEntity>
            .NewConfig()
            .Map(dest => dest, src => src.SummaryData)
            .Ignore(dest => dest.Id);
    }

    internal static SummaryEntity ToEntity(this SummaryCreateCommand command)
    {
        return command.Adapt<SummaryEntity>();
    }

    internal static SummaryModel ToModel(this SummaryEntity entity)
    {
        return entity.Adapt<SummaryModel>();
    }

    internal static SummaryPatchCommand ToUpdateRequest(this SummaryEntity entity)
    {
        return entity.Adapt<SummaryPatchCommand>();
    }

    internal static SummaryComplexCommand ToUpdateComplexRequest(this SummaryEntity entity)
    {
        return entity.Adapt<SummaryComplexCommand>();
    }

    internal static void UpdateEntity(this SummaryEntity entity, SummaryPatchCommand patchCommand)
    {
        patchCommand.Adapt(entity);
    }

    internal static void UpdateComplexEntity(this SummaryEntity entity, SummaryComplexCommand command)
    {
        command.Adapt(entity);
    }
}