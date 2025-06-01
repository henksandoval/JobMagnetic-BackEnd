using AutoFixture;
using JobMagnet.Application.UseCases.CvParser.DTO.RawDTOs;

namespace JobMagnet.Shared.Tests.Fixtures.Customizations.Raws;

public class PortfolioParseCustomization : ICustomization
{
    private static int _autoIncrementId = 1;

    public void Customize(IFixture fixture)
    {
        fixture.Customize<PortfolioGalleryRaw>(composer =>
            composer
                .Do(ApplyCommonProperties)
                .OmitAutoProperties()
        );
    }

    private static void ApplyCommonProperties(dynamic item)
    {
        item.Title = FixtureBuilder.Faker.Company.CompanyName();
        item.Description = FixtureBuilder.Faker.Lorem.Sentence();
        item.UrlLink = FixtureBuilder.Faker.Image.PicsumUrl();
        item.UrlImage = FixtureBuilder.Faker.Image.PicsumUrl();
        item.UrlVideo = FixtureBuilder.Faker.Image.PicsumUrl();
        item.Type = FixtureBuilder.Faker.Address.CountryCode();
        item.Position = _autoIncrementId++;
    }
}