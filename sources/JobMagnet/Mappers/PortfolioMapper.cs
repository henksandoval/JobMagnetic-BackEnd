using JobMagnet.Infrastructure.Entities;
using JobMagnet.Models.Portfolio;
using Mapster;

namespace JobMagnet.Mappers;

public static class PortfolioMapper
{
    public static PortfolioEntity ToEntity(PortfolioCreateRequest request)
    {
        return request.Adapt<PortfolioEntity>();
    }

    public static PortfolioModel ToModel(PortfolioEntity entity)
    {
        return entity.Adapt<PortfolioModel>();
    }

    public static PortfolioRequest ToUpdateRequest(PortfolioEntity entity)
    {
        return entity.Adapt<PortfolioRequest>();
    }

    public static void UpdateEntity(this PortfolioEntity entity, PortfolioRequest request)
    {
        request.Adapt(entity);
    }
}