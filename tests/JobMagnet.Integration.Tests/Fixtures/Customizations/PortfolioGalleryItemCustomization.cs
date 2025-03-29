using AutoFixture;
using Bogus;
using JobMagnet.Infrastructure.Entities;
using JobMagnet.Models.Portfolio;

namespace JobMagnet.Integration.Tests.Fixtures.Customizations;

public class PortfolioGalleryItemCustomization : ICustomization
{
    private static int _autoIncrementId = 1;
    private readonly Faker _faker = new();

    public void Customize(IFixture fixture)
    {
        fixture.Customize<PortfolioGalleryItemEntity>(composer =>
            composer
                .Without(x => x.Id)
                .Do(ApplyCommonProperties)
                .OmitAutoProperties());

        fixture.Customize<PortfolioGalleryItemCreateRequest>(composer =>
            composer
                .Do(ApplyCommonProperties)
                .OmitAutoProperties());
    }

    private void ApplyCommonProperties(dynamic item)
    {
        item.Title = _faker.Company.CompanyName();
        item.Description = _faker.Lorem.Sentence();
        item.UrlLink = _faker.Image.PicsumUrl();
        item.UrlImage = _faker.Image.PicsumUrl();
        item.UrlVideo = _faker.Image.PicsumUrl();
        item.Type = _faker.Address.CountryCode();
        item.Position = _autoIncrementId++;
    }
}