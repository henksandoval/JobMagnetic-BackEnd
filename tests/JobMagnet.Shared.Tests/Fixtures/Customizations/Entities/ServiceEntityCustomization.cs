using AutoFixture;
using JobMagnet.Domain.Core.Entities;

namespace JobMagnet.Shared.Tests.Fixtures.Customizations.Entities;

public class ServiceEntityCustomization : ICustomization
{
    public void Customize(IFixture fixture)
    {
        fixture.Customize<ServiceEntity>(composer =>
            composer
                .With(x => x.Id, 0)
                .With(x => x.IsDeleted, false)
                .Without(x => x.DeletedAt)
                .Without(x => x.DeletedBy)
                .With(x => x.Profile, fixture.Create<ProfileEntity>())
                .With(x => x.ProfileId, 0)
                .Do(ApplyCommonProperties)
                .OmitAutoProperties()
        );
    }

    private static void ApplyCommonProperties(dynamic item)
    {
        item.Overview = FixtureBuilder.Faker.Lorem.Paragraph();
        item.GalleryItems = FixtureBuilder.Build().CreateMany<ServiceGalleryItemEntity>().ToList();
    }
}