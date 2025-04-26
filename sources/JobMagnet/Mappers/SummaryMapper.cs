using JobMagnet.Infrastructure.Entities;
using JobMagnet.Models.Commands.Summary;
using Mapster;

namespace JobMagnet.Mappers;

internal static class SummaryMapper
{
    internal static SummaryEntity ToEntity(SummaryCreateRequest request)
    {
        return request.Adapt<SummaryEntity>();
    }

    internal static SummaryModel ToModel(SummaryEntity entity)
    {
        return entity.Adapt<SummaryModel>();
    }

    internal static SummaryRequest ToUpdateRequest(SummaryEntity entity)
    {
        return entity.Adapt<SummaryRequest>();
    }

    internal static SummaryComplexRequest ToUpdateComplexRequest(SummaryEntity entity)
    {
        return entity.Adapt<SummaryComplexRequest>();
    }

    internal static void UpdateEntity(this SummaryEntity entity, SummaryRequest request)
    {
        request.Adapt(entity);
    }

    internal static void UpdateComplexEntity(this SummaryEntity entity, SummaryComplexRequest request)
    {
        request.Adapt(entity);
    }
}