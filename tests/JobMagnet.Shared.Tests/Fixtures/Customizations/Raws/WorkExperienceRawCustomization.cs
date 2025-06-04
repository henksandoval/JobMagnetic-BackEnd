using AutoFixture;
using JobMagnet.Application.UseCases.CvParser.DTO.RawDTOs;
using JobMagnet.Shared.Tests.Utils;
using JobMagnet.Shared.Utils;

namespace JobMagnet.Shared.Tests.Fixtures.Customizations.Raws;

public class WorkExperienceRawCustomization : ICustomization
{
    public void Customize(IFixture fixture)
    {
        fixture.Register<WorkExperienceRaw>(() =>
            new (
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
}