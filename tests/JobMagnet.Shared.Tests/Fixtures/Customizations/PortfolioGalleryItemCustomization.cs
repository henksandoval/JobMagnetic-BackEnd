using AutoFixture;
using Bogus;
using JobMagnet.Infrastructure.Entities;
using JobMagnet.Models.Portfolio;

namespace JobMagnet.Shared.Tests.Fixtures.Customizations;

public class PortfolioGalleryItemCustomization : ICustomization
{
    private static int _autoIncrementId = 1;

    public void Customize(IFixture fixture)
    {
        fixture.Customize<PortfolioGalleryItemEntity>(composer =>
            composer
                .Without(x => x.Id)
                .Do(ApplyCommonProperties)
                .OmitAutoProperties());

        fixture.Customize<PortfolioGalleryItemRequest>(composer =>
            composer
                .Do(ApplyCommonProperties)
                .OmitAutoProperties());
    }

    private static void ApplyCommonProperties(dynamic item)
    {
        var faker = FixtureBuilder.Faker;

        item.Title = faker.Company.CompanyName();
        item.Description = faker.Lorem.Sentence();
        item.UrlLink = faker.Image.PicsumUrl();
        item.UrlImage = faker.Image.PicsumUrl();
        item.UrlVideo = faker.Image.PicsumUrl();
        item.Type = faker.Address.CountryCode();
        item.Position = _autoIncrementId++;
    }
}