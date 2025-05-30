using AutoFixture;
using JobMagnet.Application.UseCases.CvParser.ParsingDTOs;
using JobMagnet.Domain.Entities;
using JobMagnet.Shared.Tests.Utils;
using JobMagnet.Shared.Utils;

namespace JobMagnet.Shared.Tests.Fixtures.Customizations.DTO;

public class EducationParseCustomization : ICustomization
{
    public void Customize(IFixture fixture)
    {
        fixture.Customize<EducationParseDto>(composer =>
            composer
                .Do(ApplyCommonProperties)
                .OmitAutoProperties());
    }

    private static void ApplyCommonProperties(dynamic item)
    {
        item.Degree = FixtureBuilder.Faker.PickRandom(StaticCustomizations.Degrees);
        item.InstitutionName = FixtureBuilder.Faker.PickRandom(StaticCustomizations.Universities);
        item.InstitutionLocation = FixtureBuilder.Faker.Address.FullAddress();
        item.StartDate = FixtureBuilder.Faker.Date.Past(20, DateTime.Now.AddYears(-5)).ToDateOnly();
        item.EndDate = TestUtilities.OptionalValue(
            FixtureBuilder.Faker, f => f.Date.Past(20, DateTime.Now.AddYears(-5))
        ).ToDateOnly();
        item.Description = FixtureBuilder.Faker.Lorem.Sentences();
    }
}