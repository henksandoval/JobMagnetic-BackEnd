using AutoFixture;
using JobMagnet.Infrastructure.Entities;

namespace JobMagnet.Shared.Tests.Fixtures.Customizations;

public class ServiceCustomization : ICustomization
{
    public void Customize(IFixture fixture)
    {
        fixture.Customize<ServiceEntity>(composer =>
            composer
                .With(x => x.Id, 0)
                .With(x => x.Overview, FixtureBuilder.Faker.Lorem.Paragraph())
                .With(x => x.IsDeleted, false)
                .Without(x => x.DeletedAt)
                .Without(x => x.DeletedBy)
                .With(x => x.Profile, fixture.Create<ProfileEntity>())
                .With(x => x.GalleryItems, fixture.CreateMany<ServiceGalleryItemEntity>().ToList())
                .OmitAutoProperties()
        );
    }
}