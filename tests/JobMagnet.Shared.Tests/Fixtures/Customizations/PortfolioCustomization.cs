using AutoFixture;
using JobMagnet.Application.UseCases.CvParser.DTO.RawDTOs;
using JobMagnet.Domain.Core.Entities;

namespace JobMagnet.Shared.Tests.Fixtures.Customizations;

public class PortfolioCustomization : ICustomization
{
    private static int _autoIncrementId = 1;

    public void Customize(IFixture fixture)
    {
        fixture.Customize<PortfolioGalleryEntity>(composer =>
            composer
                .FromFactory(() => new PortfolioGalleryEntity(
                    FixtureBuilder.Faker.Company.CompanyName(),
                    FixtureBuilder.Faker.Lorem.Sentence(),
                    FixtureBuilder.Faker.Image.PicsumUrl(),
                    FixtureBuilder.Faker.Image.PicsumUrl(),
                    FixtureBuilder.Faker.Image.PicsumUrl(),
                    FixtureBuilder.Faker.Address.CountryCode()
                ))
                .OmitAutoProperties()
        );

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