using JobMagnet.Infrastructure.Entities;
using JobMagnet.Models.Portfolio;
using Mapster;

namespace JobMagnet.Mappers;

internal static class PortfolioMapper
{
    internal static PortfolioGalleryEntity ToEntity(PortfolioCreateRequest request)
    {
        return request.Adapt<PortfolioGalleryEntity>();
    }

    internal static PortfolioModel ToModel(PortfolioGalleryEntity galleryEntity)
    {
        return galleryEntity.Adapt<PortfolioModel>();
    }

    internal static PortfolioRequest ToUpdateRequest(PortfolioGalleryEntity galleryEntity)
    {
        return galleryEntity.Adapt<PortfolioRequest>();
    }

    internal static void UpdateEntity(this PortfolioGalleryEntity galleryEntity, PortfolioRequest request)
    {
        request.Adapt(galleryEntity);
    }
}