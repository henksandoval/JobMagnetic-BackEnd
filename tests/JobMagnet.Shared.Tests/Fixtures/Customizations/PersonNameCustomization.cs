using AutoFixture;
using Bogus;
using JobMagnet.Domain.Aggregates.Profiles.ValueObjects;
using JobMagnet.Shared.Tests.Utils;

namespace JobMagnet.Shared.Tests.Fixtures.Customizations;

public class PersonNameCustomization : ICustomization
{
    private static readonly Faker Faker = FixtureBuilder.Faker;

    public void Customize(IFixture fixture)
    {
        fixture.Customize<PersonName>(composer => composer
                .FromFactory(() =>
                {
                    var middleName = TestUtilities.OptionalValue(Faker, f => f.Name.FirstName());
                    var secondLastName = TestUtilities.OptionalValue(Faker, f => f.Name.LastName());

                    return new PersonName(FirstName(), LastName(), middleName, secondLastName);
                    string FirstName() => Faker.Name.FirstName();
                    string LastName() => Faker.Name.LastName();
                })
                .OmitAutoProperties()
        );
    }
}