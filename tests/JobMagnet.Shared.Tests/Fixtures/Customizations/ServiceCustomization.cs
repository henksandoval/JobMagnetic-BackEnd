using AutoFixture;
using JobMagnet.Application.UseCases.CvParser.DTO.RawDTOs;
using JobMagnet.Domain.Core.Entities;

namespace JobMagnet.Shared.Tests.Fixtures.Customizations;

public class ServiceCustomization : ICustomization
{
    public void Customize(IFixture fixture)
    {
        fixture.Customize<ServiceEntity>(composer =>
            composer
                .With(x => x.Id, 0)
                .With(x => x.IsDeleted, false)
                .Without(x => x.DeletedAt)
                .Without(x => x.DeletedBy)
                .Without(x => x.Profile)
                .With(x => x.ProfileId, 0)
                .Do(ApplyCommonProperties)
                .OmitAutoProperties()
        );

        fixture.Customize<ServiceRaw>(composer =>
            composer.FromFactory(() => new ServiceRaw(
                FixtureBuilder.Faker.Lorem.Paragraph(),
                FixtureBuilder.Build().CreateMany<GalleryItemRaw>().ToList()
            ))
        );
    }

    private static void ApplyCommonProperties(dynamic item)
    {
        item.Overview = FixtureBuilder.Faker.Lorem.Paragraph();
        item.GalleryItems = FixtureBuilder.Build().CreateMany<ServiceGalleryItemEntity>().ToList();
    }
}