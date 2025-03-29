using JobMagnet.Infrastructure.Entities;
using JobMagnet.Models.Portfolio;
using Mapster;

namespace JobMagnet.Mappers;

internal static class PortfolioMapper
{
    internal static PortfolioEntity ToEntity(PortfolioCreateRequest request)
    {
        return request.Adapt<PortfolioEntity>();
    }

    internal static PortfolioModel ToModel(PortfolioEntity entity)
    {
        return entity.Adapt<PortfolioModel>();
    }

    internal static PortfolioRequest ToUpdateRequest(PortfolioEntity entity)
    {
        return entity.Adapt<PortfolioRequest>();
    }

    internal static void UpdateEntity(this PortfolioEntity entity, PortfolioRequest request)
    {
        request.Adapt(entity);
    }
}