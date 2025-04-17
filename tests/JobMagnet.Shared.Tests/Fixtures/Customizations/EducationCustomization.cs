using AutoFixture;
using JobMagnet.Infrastructure.Entities;
using JobMagnet.Shared.Tests.Utils;

namespace JobMagnet.Shared.Tests.Fixtures.Customizations;

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
        var faker = FixtureBuilder.Faker;

        item.Degree = faker.PickRandom(Degrees);
        item.InstitutionName = faker.PickRandom(Universities);
        item.InstitutionLocation = faker.Address.FullAddress();
        item.StartDate = faker.Date.Past(20, DateTime.Now.AddYears(-5));
        item.EndDate = TestUtilities.OptionalValue(faker, f => f.Date.Past(20, DateTime.Now.AddYears(-5)));
        item.Description = faker.Lorem.Sentences();
    }
}