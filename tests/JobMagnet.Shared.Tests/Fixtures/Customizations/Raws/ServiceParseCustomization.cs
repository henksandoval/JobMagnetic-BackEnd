using AutoFixture;
using JobMagnet.Application.UseCases.CvParser.DTO.RawDTOs;

namespace JobMagnet.Shared.Tests.Fixtures.Customizations.Raws;

public class ServiceParseCustomization : ICustomization
{
    public void Customize(IFixture fixture)
    {
        fixture.Customize<ServiceRaw>(composer =>
            composer
                .Do(ApplyCommonProperties)
                .OmitAutoProperties()
        );
    }

    private static void ApplyCommonProperties(dynamic item)
    {
        item.Overview = FixtureBuilder.Faker.Lorem.Paragraph();
        item.GalleryItems = FixtureBuilder.Build().CreateMany<GalleryItemRaw>().ToList();
    }
}