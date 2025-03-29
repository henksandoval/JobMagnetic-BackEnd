using JobMagnet.Infrastructure.Entities;
using JobMagnet.Models.Portfolio;
using Mapster;

namespace JobMagnet.Mappers;

public static class PortfolioMapper
{
    static PortfolioMapper()
    {
        TypeAdapterConfig<PortfolioUpdateRequest, PortfolioEntity>.NewConfig()
            .Ignore(destination => destination.Id);
    }

    public static PortfolioEntity ToEntity(PortfolioCreateRequest request)
    {
        return request.Adapt<PortfolioEntity>();
    }

    public static PortfolioModel ToModel(PortfolioEntity entity)
    {
        return entity.Adapt<PortfolioModel>();
    }

    public static PortfolioUpdateRequest ToUpdateRequest(PortfolioEntity entity)
    {
        return entity.Adapt<PortfolioUpdateRequest>();
    }

    public static void UpdateEntity(this PortfolioEntity entity, PortfolioUpdateRequest request)
    {
        request.Adapt(entity);
    }
}