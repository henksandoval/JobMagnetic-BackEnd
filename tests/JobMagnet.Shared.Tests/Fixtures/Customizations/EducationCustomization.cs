using AutoFixture;
using Bogus;
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

    private readonly Faker _faker = new();

    public void Customize(IFixture fixture)
    {
        fixture.Customize<EducationEntity>(composer =>
            composer
                .Without(x => x.Id)
                .Do(ApplyCommonProperties)
                .OmitAutoProperties());
    }

    private void ApplyCommonProperties(dynamic item)
    {
        item.Degree = _faker.PickRandom(Degrees);
        item.InstitutionName = _faker.PickRandom(Universities);
        item.InstitutionLocation = _faker.Address.FullAddress();
        item.StartDate = _faker.Date.Past(20, DateTime.Now.AddYears(-5));
        item.EndDate = TestUtilities.OptionalValue(_faker, f => f.Date.Past(20, DateTime.Now.AddYears(-5)));
        item.Description = _faker.Lorem.Sentences();
    }
}