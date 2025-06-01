using AutoFixture;
using JobMagnet.Application.UseCases.CvParser.DTO.RawDTOs;

namespace JobMagnet.Shared.Tests.Fixtures.Customizations.Raws;

public class PortfolioRawCustomization : ICustomization
{
    public void Customize(IFixture fixture)
    {
        fixture.Customize<PortfolioGalleryRaw>(composer =>
            composer.FromFactory(() => new PortfolioGalleryRaw(
                FixtureBuilder.Faker.Company.CompanyName(),
                FixtureBuilder.Faker.Lorem.Sentence(),
                FixtureBuilder.Faker.Image.PicsumUrl(),
                FixtureBuilder.Faker.Image.PicsumUrl(),
                FixtureBuilder.Faker.Image.PicsumUrl(),
                FixtureBuilder.Faker.Address.CountryCode()
            ))
        );
    }
}