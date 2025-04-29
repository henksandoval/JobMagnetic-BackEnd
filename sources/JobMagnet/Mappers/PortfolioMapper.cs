using JobMagnet.Infrastructure.Entities;
using JobMagnet.Models.Commands.Portfolio;
using JobMagnet.Models.Responses.Portfolio;
using Mapster;

namespace JobMagnet.Mappers;

internal static class PortfolioMapper
{
    static PortfolioMapper()
    {
        TypeAdapterConfig<PortfolioGalleryEntity, PortfolioModel>
            .NewConfig()
            .Map(dest => dest.PortfolioData, src => src);

        TypeAdapterConfig<PortfolioCreateCommand, PortfolioGalleryEntity>
            .NewConfig()
            .Map(dest => dest, src => src.PortfolioData);

        TypeAdapterConfig<PortfolioGalleryEntity, PortfolioUpdateCommand>
            .NewConfig()
            .Map(dest => dest.PortfolioData, src => src);

        TypeAdapterConfig<PortfolioUpdateCommand, PortfolioGalleryEntity>
            .NewConfig()
            .Map(dest => dest, src => src.PortfolioData);
    }

    internal static PortfolioGalleryEntity ToEntity(this PortfolioCreateCommand command)
    {
        return command.Adapt<PortfolioGalleryEntity>();
    }

    internal static PortfolioModel ToModel(this PortfolioGalleryEntity galleryEntity)
    {
        return galleryEntity.Adapt<PortfolioModel>();
    }

    internal static PortfolioUpdateCommand ToUpdateRequest(this PortfolioGalleryEntity galleryEntity)
    {
        return galleryEntity.Adapt<PortfolioUpdateCommand>();
    }

    internal static void UpdateEntity(this PortfolioGalleryEntity galleryEntity, PortfolioUpdateCommand updateCommand)
    {
        updateCommand.Adapt(galleryEntity);
    }
}