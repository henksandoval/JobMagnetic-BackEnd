using AutoFixture;
using JobMagnet.Domain.Core.Entities;
using JobMagnet.Shared.Tests.Utils;

namespace JobMagnet.Shared.Tests.Fixtures.Customizations.Entities;

public class EducationEntityCustomization : ICustomization
{
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
        item.Degree = FixtureBuilder.Faker.PickRandom(StaticCustomizations.Degrees);
        item.InstitutionName = FixtureBuilder.Faker.PickRandom(StaticCustomizations.Universities);
        item.InstitutionLocation = FixtureBuilder.Faker.Address.FullAddress();
        item.StartDate = FixtureBuilder.Faker.Date.Past(20, DateTime.Now.AddYears(-5));
        item.EndDate =
            TestUtilities.OptionalValue(FixtureBuilder.Faker, f => f.Date.Past(20, DateTime.Now.AddYears(-5)));
        item.Description = FixtureBuilder.Faker.Lorem.Sentences();
    }
}