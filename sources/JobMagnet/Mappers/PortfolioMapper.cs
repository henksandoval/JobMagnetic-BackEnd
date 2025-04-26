using JobMagnet.Infrastructure.Entities;
using JobMagnet.Models.Commands.Portfolio;
using JobMagnet.Models.Responses.Portfolio;
using Mapster;

namespace JobMagnet.Mappers;

internal static class PortfolioMapper
{
    internal static PortfolioGalleryEntity ToEntity(PortfolioCreateCommand command)
    {
        return command.Adapt<PortfolioGalleryEntity>();
    }

    internal static PortfolioModel ToModel(PortfolioGalleryEntity galleryEntity)
    {
        return galleryEntity.Adapt<PortfolioModel>();
    }

    internal static PortfolioUpdateCommand ToUpdateRequest(PortfolioGalleryEntity galleryEntity)
    {
        return galleryEntity.Adapt<PortfolioUpdateCommand>();
    }

    internal static void UpdateEntity(this PortfolioGalleryEntity galleryEntity, PortfolioUpdateCommand updateCommand)
    {
        updateCommand.Adapt(galleryEntity);
    }
}