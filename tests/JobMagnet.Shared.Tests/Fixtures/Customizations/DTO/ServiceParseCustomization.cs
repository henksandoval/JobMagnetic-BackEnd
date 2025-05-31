using AutoFixture;
using JobMagnet.Application.UseCases.CvParser.ParsingDTOs;

namespace JobMagnet.Shared.Tests.Fixtures.Customizations.DTO;

public class ServiceParseCustomization : ICustomization
{
    public void Customize(IFixture fixture)
    {
        fixture.Customize<ServiceParseDto>(composer =>
            composer
                .Do(ApplyCommonProperties)
                .OmitAutoProperties()
        );
    }

    private static void ApplyCommonProperties(dynamic item)
    {
        item.Overview = FixtureBuilder.Faker.Lorem.Paragraph();
        item.GalleryItems = FixtureBuilder.Build().CreateMany<GalleryItemParseDto>().ToList();
    }
}