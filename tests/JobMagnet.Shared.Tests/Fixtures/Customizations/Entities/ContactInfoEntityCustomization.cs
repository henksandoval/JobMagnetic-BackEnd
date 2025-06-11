using AutoFixture;
using JobMagnet.Domain.Core.Entities;

namespace JobMagnet.Shared.Tests.Fixtures.Customizations.Entities;

public class ContactInfoEntityCustomization : ICustomization
{
    public void Customize(IFixture fixture)
    {
        fixture.Customize<ContactInfoEntity>(composer => composer
            .With(x => x.Id, 0)
            .With(x => x.IsDeleted, false)
            .Without(x => x.DeletedAt)
            .Without(x => x.DeletedBy)
            .Without(x => x.Resume)
            .Do(ApplyCommonProperties)
            .OmitAutoProperties());
    }

    private static void ApplyCommonProperties(dynamic item)
    {
        var contactTypeName = FixtureBuilder.Faker.PickRandom(StaticCustomizations.ContactTypes).Name;
        item.Value = FixtureBuilder.Faker.Phone.PhoneNumber();
        item.ContactType = new ContactTypeEntity(contactTypeName);
    }
}
