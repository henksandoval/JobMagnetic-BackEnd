using AutoFixture;
using JobMagnet.Application.Models.Base;
using JobMagnet.Domain.Entities;

namespace JobMagnet.Shared.Tests.Fixtures.Customizations;

public class ServiceGalleryItemCustomization : ICustomization
{
    private static int _autoIncrementId = 1;

    public void Customize(IFixture fixture)
    {
        fixture.Customize<ServiceGalleryItemBase>(composer =>
            composer
                .With(x => x.Id, 0)
                .With(x => x.Title, FixtureBuilder.Faker.Company.CompanyName())
                .With(x => x.Description, FixtureBuilder.Faker.Lorem.Sentence())
                .With(x => x.UrlLink, FixtureBuilder.Faker.Image.PicsumUrl())
                .With(x => x.UrlImage, FixtureBuilder.Faker.Image.PicsumUrl())
                .With(x => x.UrlVideo, FixtureBuilder.Faker.Image.PicsumUrl())
                .With(x => x.Type, FixtureBuilder.Faker.Address.CountryCode())
                .WithAutoProperties()
        );

        fixture.Customize<ServiceBase>(composer =>
            composer.WithAutoProperties()
        );

        fixture.Customize<ServiceGalleryItemEntity>(composer =>
            composer
                .Without(x => x.Id)
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
        item.Position = _autoIncrementId++;
    }
}