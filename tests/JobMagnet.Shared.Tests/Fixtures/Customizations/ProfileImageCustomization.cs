using AutoFixture;
using Bogus;
using JobMagnet.Domain.Aggregates.Profiles.ValueObjects;

namespace JobMagnet.Shared.Tests.Fixtures.Customizations;

public class ProfileImageCustomization : ICustomization
{
    private static readonly Faker Faker = FixtureBuilder.Faker;

    public void Customize(IFixture fixture)
    {
        fixture.Customize<ProfileImage>(
            composer => composer
                .FromFactory(() => new ProfileImage(Faker.Internet.Url()))
                .OmitAutoProperties()
        );
    }
}