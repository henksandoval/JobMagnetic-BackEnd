using AutoFixture;
using AutoFixture.Dsl;
using JobMagnet.Infrastructure.Entities;

namespace JobMagnet.Shared.Tests.Fixtures.Builders;

public static class PortfolioFixtureBuilder
{
    public static IPostprocessComposer<PortfolioEntity> GetPortfolioEntityComposer(this IFixture fixture, int galleryItems = 5)
    {
        var portfolioGalleryItems = fixture.CreateMany<PortfolioGalleryItemEntity>(galleryItems).ToList();
        var portfolioEntity = fixture.Build<PortfolioEntity>()
            .With(x => x.Id, 0)
            .With(x => x.IsDeleted, false)
            .Without(x => x.DeletedAt)
            .Without(x => x.DeletedBy)
            .With(x => x.Profile, fixture.CreateProfileEntity)
            .With(x => x.GalleryItems, portfolioGalleryItems);

        return portfolioEntity;
    }
}