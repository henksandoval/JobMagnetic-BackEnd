using AutoFixture;
using JobMagnet.Application.UseCases.CvParser.DTO.RawDTOs;
using JobMagnet.Domain.Core.Entities;
using JobMagnet.Shared.Tests.Utils;
using JobMagnet.Shared.Utils;

namespace JobMagnet.Shared.Tests.Fixtures.Customizations;

public class WorkExperienceCustomization : ICustomization
{
    public void Customize(IFixture fixture)
    {
        fixture.Customize<WorkExperienceEntity>(composer =>
            composer
                .Without(x => x.Id)
                .Do(ApplyCommonProperties)
                .OmitAutoProperties());

        fixture.Register(() =>
            new WorkExperienceRaw(
                FixtureBuilder.Faker.PickRandom(StaticCustomizations.JobTitles),
                FixtureBuilder.Faker.PickRandom(StaticCustomizations.CompanyNames),
                FixtureBuilder.Faker.Address.FullAddress(),
                FixtureBuilder.Faker.Date.Past(20, DateTime.Now.AddYears(-5)).ToDateOnly().ToString(),
                TestUtilities
                    .OptionalValue<DateTime?>(FixtureBuilder.Faker, f => f.Date.Past(20, DateTime.Now.AddYears(-5)), 75)
                    ?.ToShortDateString() ?? string.Empty,
                FixtureBuilder.Faker.PickRandom(StaticCustomizations.Descriptions),
                []
            )
        );
    }

    private static void ApplyCommonProperties(dynamic item)
    {
        item.JobTitle = FixtureBuilder.Faker.PickRandom(StaticCustomizations.JobTitles);
        item.CompanyName = FixtureBuilder.Faker.PickRandom(StaticCustomizations.CompanyNames);
        item.CompanyLocation = FixtureBuilder.Faker.Address.FullAddress();
        item.StartDate = FixtureBuilder.Faker.Date.Past(20, DateTime.Now.AddYears(-5));
        item.EndDate =
            TestUtilities.OptionalValue(FixtureBuilder.Faker, f => f.Date.Past(20, DateTime.Now.AddYears(-5)));
        item.Description = FixtureBuilder.Faker.PickRandom(StaticCustomizations.Descriptions);
    }
}