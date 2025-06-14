﻿using JobMagnet.Application.Contracts.Commands.Portfolio;
using JobMagnet.Application.Contracts.Responses.Portfolio;
using JobMagnet.Domain.Core.Entities;
using Mapster;

namespace JobMagnet.Application.Mappers;

public static class PortfolioMapper
{
    static PortfolioMapper()
    {
        TypeAdapterConfig<PortfolioGalleryEntity, PortfolioResponse>
            .NewConfig()
            .Map(dest => dest.PortfolioData, src => src);

        TypeAdapterConfig<PortfolioCommand, PortfolioGalleryEntity>
            .NewConfig()
            .Map(dest => dest, src => src.PortfolioData);

        TypeAdapterConfig<PortfolioGalleryEntity, PortfolioCommand>
            .NewConfig()
            .Map(dest => dest.PortfolioData, src => src);
    }

    public static PortfolioGalleryEntity ToEntity(this PortfolioCommand command)
    {
        return command.Adapt<PortfolioGalleryEntity>();
    }

    public static PortfolioResponse ToModel(this PortfolioGalleryEntity galleryEntity)
    {
        return galleryEntity.Adapt<PortfolioResponse>();
    }

    public static PortfolioCommand ToUpdateRequest(this PortfolioGalleryEntity galleryEntity)
    {
        return galleryEntity.Adapt<PortfolioCommand>();
    }

    public static void UpdateEntity(this PortfolioGalleryEntity galleryEntity, PortfolioCommand updateCommand)
    {
        updateCommand.Adapt(galleryEntity);
    }
}