using JobMagnet.Infrastructure.Entities;
using JobMagnet.Models.Commands.Portfolio;
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

    internal static PortfolioUpdateRequest ToUpdateRequest(PortfolioGalleryEntity galleryEntity)
    {
        return galleryEntity.Adapt<PortfolioUpdateRequest>();
    }

    internal static void UpdateEntity(this PortfolioGalleryEntity galleryEntity, PortfolioUpdateRequest updateRequest)
    {
        updateRequest.Adapt(galleryEntity);
    }
}