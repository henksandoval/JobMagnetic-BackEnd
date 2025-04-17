using AutoFixture;
using JobMagnet.Infrastructure.Entities;

namespace JobMagnet.Shared.Tests.Fixtures.Customizations;

public class PortfolioCustomization : ICustomization
{
    public void Customize(IFixture fixture)
    {
        fixture.Customize<PortfolioEntity>(composer =>
            composer
                .With(x => x.Id, 0)
                .With(x => x.IsDeleted, false)
                .Without(x => x.DeletedAt)
                .Without(x => x.DeletedBy)
                .With(x => x.ProfileId, 0)
                .With(x => x.Profile, fixture.Create<ProfileEntity>())
                .With(x => x.GalleryItems, fixture.CreateMany<PortfolioGalleryItemEntity>().ToList())
                .OmitAutoProperties()
        );
    }
}