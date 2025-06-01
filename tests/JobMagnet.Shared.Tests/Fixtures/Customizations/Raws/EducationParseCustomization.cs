using AutoFixture;
using JobMagnet.Application.UseCases.CvParser.DTO.RawDTOs;
using JobMagnet.Shared.Tests.Utils;
using JobMagnet.Shared.Utils;

namespace JobMagnet.Shared.Tests.Fixtures.Customizations.Raws;

public class EducationParseCustomization : ICustomization
{
    public void Customize(IFixture fixture)
    {
        fixture.Customize<EducationRaw>(composer =>
            composer
                .Do(ApplyCommonProperties)
                .OmitAutoProperties());
    }

    private static void ApplyCommonProperties(dynamic item)
    {
        item.Degree = FixtureBuilder.Faker.PickRandom(StaticCustomizations.Degrees);
        item.InstitutionName = FixtureBuilder.Faker.PickRandom(StaticCustomizations.Universities);
        item.InstitutionLocation = FixtureBuilder.Faker.Address.FullAddress();
        item.StartDate = FixtureBuilder.Faker.Date.Past(20, DateTime.Now.AddYears(-5)).ToDateOnly().ToString();
        item.EndDate = TestUtilities.OptionalValue<DateTime?>(
            FixtureBuilder.Faker, f => f.Date.Past(20, DateTime.Now.AddYears(-5)), 80
        )?.ToShortDateString() ?? string.Empty;
        item.Description = FixtureBuilder.Faker.Lorem.Sentences();
    }
}