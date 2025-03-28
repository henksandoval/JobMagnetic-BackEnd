using AutoFixture;
using Bogus;
using JobMagnet.Infrastructure.Entities;

namespace JobMagnet.Integration.Tests.Fixtures.Customizations;

public class PortfolioGalleryItemCustomization : ICustomization
{
    private static int _autoIncrementId = 1;

    public void Customize(IFixture fixture)
    {
        var faker = new Faker();

        fixture.Customize<PortfolioGalleryItemEntity>(composer => composer
            .Without(x => x.Id)
            .Do(x => x.Title = faker.Company.CompanyName())
            .Do(x => x.Description = faker.Lorem.Sentence())
            .Do(x => x.UrlLink = faker.Image.PicsumUrl())
            .Do(x => x.UrlImage = faker.Image.PicsumUrl())
            .Do(x => x.UrlVideo = faker.Image.PicsumUrl())
            .Do(x => x.Type = faker.Address.CountryCode())
            .Do(x => x.Position = _autoIncrementId++)
            .OmitAutoProperties());
    }
}