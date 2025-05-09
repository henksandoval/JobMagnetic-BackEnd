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

        TypeAdapterConfig<PortfolioCommand, PortfolioGalleryEntity>
            .NewConfig()
            .Map(dest => dest, src => src.PortfolioData);

        TypeAdapterConfig<PortfolioGalleryEntity, PortfolioCommand>
            .NewConfig()
            .Map(dest => dest.PortfolioData, src => src);
    }

    internal static PortfolioGalleryEntity ToEntity(this PortfolioCommand command)
    {
        return command.Adapt<PortfolioGalleryEntity>();
    }

    internal static PortfolioModel ToModel(this PortfolioGalleryEntity galleryEntity)
    {
        return galleryEntity.Adapt<PortfolioModel>();
    }

    internal static PortfolioCommand ToUpdateRequest(this PortfolioGalleryEntity galleryEntity)
    {
        return galleryEntity.Adapt<PortfolioCommand>();
    }

    internal static void UpdateEntity(this PortfolioGalleryEntity galleryEntity, PortfolioCommand updateCommand)
    {
        updateCommand.Adapt(galleryEntity);
    }
}