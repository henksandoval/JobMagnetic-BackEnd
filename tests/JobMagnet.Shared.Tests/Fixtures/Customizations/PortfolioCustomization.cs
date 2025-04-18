using AutoFixture;
using JobMagnet.Infrastructure.Entities;

namespace JobMagnet.Shared.Tests.Fixtures.Customizations;

public class PortfolioCustomization : ICustomization
{
    public void Customize(IFixture fixture)
    {
        fixture.Customize<PortfolioGalleryEntity>(composer =>
            composer
                .With(x => x.Id, 0)
                .With(x => x.IsDeleted, false)
                .Without(x => x.DeletedAt)
                .Without(x => x.DeletedBy)
                .With(x => x.ProfileId, 0)
                .With(x => x.Profile, fixture.Create<ProfileEntity>())
                .Do(ApplyCommonProperties)
                .OmitAutoProperties()
        );
    }

    private static void ApplyCommonProperties(dynamic item)
    {
        item.GalleryItems = FixtureBuilder.Build().CreateMany<PortfolioGalleryItemEntityToRemove>().ToList();
    }
}