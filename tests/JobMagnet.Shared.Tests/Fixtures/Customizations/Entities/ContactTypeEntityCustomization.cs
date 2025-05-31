using AutoFixture;
using JobMagnet.Domain.Core.Entities;

namespace JobMagnet.Shared.Tests.Fixtures.Customizations.Entities;

public class ContactTypeEntityCustomization : ICustomization
{
    public void Customize(IFixture fixture)
    {
        fixture.Customize<ContactTypeEntity>(composer => composer
            .Without(x => x.Id)
            .Do(ApplyCommonProperties)
            .OmitAutoProperties()
        );
    }

    private static void ApplyCommonProperties(dynamic item)
    {
        item.Name = FixtureBuilder.Faker.PickRandom(StaticCustomizations.ContactTypes).Name;
        item.IconClass = FixtureBuilder.Faker.PickRandom(StaticCustomizations.ContactTypes).IconClass;
    }
}