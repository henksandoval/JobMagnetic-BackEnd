using AutoFixture;
using JobMagnet.Application.UseCases.CvParser.ParsingDTOs;
using JobMagnet.Shared.Tests.Utils;
using JobMagnet.Shared.Utils;

namespace JobMagnet.Shared.Tests.Fixtures.Customizations.DTO;

public class WorkExperienceParseCustomization : ICustomization
{
    public void Customize(IFixture fixture)
    {
        fixture.Customize<WorkExperienceParseDto>(composer =>
            composer
                .Do(ApplyCommonProperties)
                .OmitAutoProperties());
    }

    private static void ApplyCommonProperties(dynamic item)
    {
        item.JobTitle = FixtureBuilder.Faker.PickRandom(StaticCustomizations.JobTitles);
        item.CompanyName = FixtureBuilder.Faker.PickRandom(StaticCustomizations.CompanyNames);
        item.CompanyLocation = FixtureBuilder.Faker.Address.FullAddress();
        item.StartDate =
            FixtureBuilder.Faker.Date.Past(20, DateTime.Now.AddYears(-5)).ToDateOnly();
        item.EndDate = TestUtilities.OptionalValue(
            FixtureBuilder.Faker, f => f.Date.Past(20, DateTime.Now.AddYears(-5))
        ).ToDateOnly();
        item.Description = FixtureBuilder.Faker.PickRandom(StaticCustomizations.Descriptions);
    }
}