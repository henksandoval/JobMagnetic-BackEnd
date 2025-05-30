using AutoFixture;
using JobMagnet.Domain.Entities;
using JobMagnet.Shared.Tests.Utils;

namespace JobMagnet.Shared.Tests.Fixtures.Customizations.Entities;

public class EducationCustomization : ICustomization
{
    private static readonly string[] Degrees =
    [
        "Bachelor's in Computer Science",
        "Master's in Business Administration",
        "PhD in Physics",
        "Associate Degree in Psychology",
        "Bachelor's in Mechanical Engineering",
        "Diploma in Graphic Design",
        "MBA in Marketing"
    ];

    private static readonly string[] Universities =
    [
        "Harvard University",
        "Stanford University",
        "Massachusetts Institute of Technology",
        "University of Cambridge",
        "University of Oxford",
        "California Institute of Technology",
        "University of Tokyo",
        "National University of Singapore",
        "University of Toronto",
        "University of Melbourne"
    ];

    public void Customize(IFixture fixture)
    {
        fixture.Customize<EducationEntity>(composer =>
            composer
                .Without(x => x.Id)
                .Do(ApplyCommonProperties)
                .OmitAutoProperties());
    }

    private static void ApplyCommonProperties(dynamic item)
    {
        item.Degree = FixtureBuilder.Faker.PickRandom(Degrees);
        item.InstitutionName = FixtureBuilder.Faker.PickRandom(Universities);
        item.InstitutionLocation = FixtureBuilder.Faker.Address.FullAddress();
        item.StartDate = FixtureBuilder.Faker.Date.Past(20, DateTime.Now.AddYears(-5));
        item.EndDate =
            TestUtilities.OptionalValue(FixtureBuilder.Faker, f => f.Date.Past(20, DateTime.Now.AddYears(-5)));
        item.Description = FixtureBuilder.Faker.Lorem.Sentences();
    }
}