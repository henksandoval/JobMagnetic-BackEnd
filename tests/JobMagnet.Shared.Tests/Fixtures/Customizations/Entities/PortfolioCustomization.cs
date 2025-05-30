using AutoFixture;
using JobMagnet.Domain.Entities;

namespace JobMagnet.Shared.Tests.Fixtures.Customizations;

public class PortfolioCustomization : ICustomization
{
    private static int _autoIncrementId = 1;

    public void Customize(IFixture fixture)
    {
        fixture.Customize<PortfolioGalleryEntity>(composer =>
            composer
                .With(x => x.Id, 0)
                .With(x => x.IsDeleted, false)
                .Without(x => x.DeletedAt)
                .Without(x => x.DeletedBy)
                .With(x => x.ProfileId, 0)
                .With(x => x.Profile, fixture.Create<ProfileEntity>())
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