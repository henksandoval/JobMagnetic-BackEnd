using AutoFixture;
using JobMagnet.Domain.Core.Entities;
using JobMagnet.Shared.Tests.Utils;

namespace JobMagnet.Shared.Tests.Fixtures.Customizations.Entities;

public class WorkExperienceEntityCustomization : ICustomization
{
    public void Customize(IFixture fixture)
    {
        fixture.Customize<WorkExperienceEntity>(composer =>
            composer
                .Without(x => x.Id)
                .Do(ApplyCommonProperties)
                .OmitAutoProperties());
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