using AutoFixture;
using JobMagnet.Application.UseCases.CvParser.RawDTOs;
using JobMagnet.Shared.Tests.Utils;
using JobMagnet.Shared.Utils;

namespace JobMagnet.Shared.Tests.Fixtures.Customizations.Raws;

public class WorkExperienceParseCustomization : ICustomization
{
    public void Customize(IFixture fixture)
    {
        fixture.Customize<WorkExperienceRaw>(composer =>
            composer
                .Do(ApplyCommonProperties)
                .OmitAutoProperties());
    }

    private static void ApplyCommonProperties(dynamic item)
    {
        item.JobTitle = FixtureBuilder.Faker.PickRandom(StaticCustomizations.JobTitles);
        item.CompanyName = FixtureBuilder.Faker.PickRandom(StaticCustomizations.CompanyNames);
        item.CompanyLocation = FixtureBuilder.Faker.Address.FullAddress();
        item.StartDate = FixtureBuilder.Faker.Date.Past(20, DateTime.Now.AddYears(-5)).ToDateOnly().ToString();
        item.EndDate = TestUtilities.OptionalValue<DateTime?>(
            FixtureBuilder.Faker, f => f.Date.Past(20, DateTime.Now.AddYears(-5)), 75
        )?.ToShortDateString() ?? string.Empty;
        item.Description = FixtureBuilder.Faker.PickRandom(StaticCustomizations.Descriptions);
    }
}