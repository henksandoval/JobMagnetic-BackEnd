using JobMagnet.Infrastructure.Entities;
using JobMagnet.Models.Portfolio;
using Mapster;

namespace JobMagnet.Mappers;

public class PortfolioMapper
{
    public static PortfolioEntity ToEntity(PortfolioCreateRequest request)
    {
        return request.Adapt<PortfolioEntity>();
    }

    public static PortfolioModel ToModel(PortfolioEntity entity)
    {
        return entity.Adapt<PortfolioModel>();
    }
}