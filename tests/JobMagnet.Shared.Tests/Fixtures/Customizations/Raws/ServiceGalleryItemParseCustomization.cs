using AutoFixture;
using JobMagnet.Application.UseCases.CvParser.DTO.RawDTOs;

namespace JobMagnet.Shared.Tests.Fixtures.Customizations.Raws;

public class ServiceGalleryItemParseCustomization : ICustomization
{
    public void Customize(IFixture fixture)
    {
        fixture.Customize<GalleryItemRaw>(composer =>
            composer.FromFactory(() => new GalleryItemRaw(
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