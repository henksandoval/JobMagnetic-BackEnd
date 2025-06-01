using AutoFixture;
using JobMagnet.Application.UseCases.CvParser.DTO.RawDTOs;
using JobMagnet.Shared.Tests.Utils;
using JobMagnet.Shared.Utils;

namespace JobMagnet.Shared.Tests.Fixtures.Customizations.Raws;

public class EducationRawCustomization : ICustomization
{
    public void Customize(IFixture fixture)
    {
        fixture.Register<EducationRaw>(() =>
            new (
                FixtureBuilder.Faker.PickRandom(StaticCustomizations.Degrees),
                FixtureBuilder.Faker.PickRandom(StaticCustomizations.Universities),
                FixtureBuilder.Faker.Address.FullAddress(),
                FixtureBuilder.Faker.Lorem.Sentences(),
                FixtureBuilder.Faker.Date.Past(20, DateTime.Now.AddYears(-5)).ToDateOnly().ToString(),
                TestUtilities.OptionalValue<DateTime?>(
                    FixtureBuilder.Faker, f => f.Date.Past(20, DateTime.Now.AddYears(-5)), 80
                )?.ToShortDateString() ?? string.Empty
            )
        );
    }
}