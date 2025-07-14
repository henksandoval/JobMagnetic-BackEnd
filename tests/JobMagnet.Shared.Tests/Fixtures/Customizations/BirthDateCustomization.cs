using AutoFixture;
using Bogus;
using JobMagnet.Domain.Aggregates.Profiles.ValueObjects;

namespace JobMagnet.Shared.Tests.Fixtures.Customizations;

public enum AgeGroup
{
    Adult,
    Minor
}

public class BirthDateCustomization(AgeGroup ageGroup = AgeGroup.Adult) : ICustomization
{
    private static readonly Faker Faker = FixtureBuilder.Faker;

    public void Customize(IFixture fixture)
    {
        fixture.Customize<BirthDate>(composer => composer
            .FromFactory(CreateBirthDate)
            .OmitAutoProperties()
        );
    }

    private BirthDate CreateBirthDate()
    {
        var (minAge, maxAge) = GetAgeRange(ageGroup);

        var today = DateTime.UtcNow;
        var maxBirthDate = today.AddYears(-minAge);
        var minBirthDate = today.AddYears(-maxAge);

        var birthDateTime = Faker.Date.Between(minBirthDate, maxBirthDate);
        return new BirthDate(DateOnly.FromDateTime(birthDateTime));
    }

    private static (int minAge, int maxAge) GetAgeRange(AgeGroup ageGroup)
    {
        return ageGroup switch
        {
            AgeGroup.Adult => (18, BirthDate.MaximumAge),
            AgeGroup.Minor => (BirthDate.MinimumAge, 17),
            _ => throw new ArgumentOutOfRangeException(nameof(ageGroup), ageGroup, null)
        };
    }
}