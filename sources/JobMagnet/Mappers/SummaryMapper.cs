using JobMagnet.Infrastructure.Entities;
using JobMagnet.Models.Commands.Summary;
using Mapster;

namespace JobMagnet.Mappers;

internal static class SummaryMapper
{
    internal static SummaryEntity ToEntity(SummaryCreateCommand command)
    {
        return command.Adapt<SummaryEntity>();
    }

    internal static SummaryModel ToModel(SummaryEntity entity)
    {
        return entity.Adapt<SummaryModel>();
    }

    internal static SummaryPatchCommand ToUpdateRequest(SummaryEntity entity)
    {
        return entity.Adapt<SummaryPatchCommand>();
    }

    internal static SummaryComplexRequest ToUpdateComplexRequest(SummaryEntity entity)
    {
        return entity.Adapt<SummaryComplexRequest>();
    }

    internal static void UpdateEntity(this SummaryEntity entity, SummaryPatchCommand patchCommand)
    {
        patchCommand.Adapt(entity);
    }

    internal static void UpdateComplexEntity(this SummaryEntity entity, SummaryComplexRequest request)
    {
        request.Adapt(entity);
    }
}