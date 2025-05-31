using AutoFixture;
using JobMagnet.Application.UseCases.CvParser.ParsingDTOs;

namespace JobMagnet.Shared.Tests.Fixtures.Customizations.DTO;

public class ServiceGalleryItemParseCustomization : ICustomization
{
    public void Customize(IFixture fixture)
    {
        fixture.Customize<GalleryItemParseDto>(composer =>
            composer
                .Do(ApplyCommonProperties)
                .OmitAutoProperties());
    }

    private static void ApplyCommonProperties(dynamic item)
    {
        item.Title = FixtureBuilder.Faker.Company.CompanyName();
        item.Description = FixtureBuilder.Faker.Lorem.Sentence();
        item.UrlLink = FixtureBuilder.Faker.Image.PicsumUrl();
        item.UrlImage = FixtureBuilder.Faker.Image.PicsumUrl();
        item.UrlVideo = FixtureBuilder.Faker.Image.PicsumUrl();
        item.Type = FixtureBuilder.Faker.Address.CountryCode();
    }
}