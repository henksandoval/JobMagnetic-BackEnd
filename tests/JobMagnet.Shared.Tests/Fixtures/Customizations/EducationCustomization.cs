using AutoFixture;
using JobMagnet.Application.UseCases.CvParser.DTO.RawDTOs;
using JobMagnet.Domain.Core.Entities;
using JobMagnet.Shared.Tests.Utils;
using JobMagnet.Shared.Utils;

namespace JobMagnet.Shared.Tests.Fixtures.Customizations;

public class EducationCustomization : ICustomization
{
    public void Customize(IFixture fixture)
    {
        fixture.Customize<EducationEntity>(composer =>
            composer
                .Without(x => x.Id)
                .Do(ApplyCommonProperties)
                .OmitAutoProperties());

        fixture.Register(() =>
            new EducationRaw(
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

    private static void ApplyCommonProperties(dynamic item)
    {
        item.Degree = FixtureBuilder.Faker.PickRandom(StaticCustomizations.Degrees);
        item.InstitutionName = FixtureBuilder.Faker.PickRandom(StaticCustomizations.Universities);
        item.InstitutionLocation = FixtureBuilder.Faker.Address.FullAddress();
        item.StartDate = FixtureBuilder.Faker.Date.Past(20, DateTime.Now.AddYears(-5));
        item.EndDate =
            TestUtilities.OptionalValue(FixtureBuilder.Faker, f => f.Date.Past(20, DateTime.Now.AddYears(-5)));
        item.Description = FixtureBuilder.Faker.Lorem.Sentences();
    }
}