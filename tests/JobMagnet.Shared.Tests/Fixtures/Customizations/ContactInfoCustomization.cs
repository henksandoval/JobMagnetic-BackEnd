using AutoFixture;
using JobMagnet.Infrastructure.Entities;

namespace JobMagnet.Shared.Tests.Fixtures.Customizations;

public class ContactInfoCustomization : ICustomization
{
    public void Customize(IFixture fixture)
    {
        fixture.Customize<ContactInfoEntity>(composer =>
            composer
                .With(x => x.Id, 0)
                .With(x => x.IsDeleted, false)
                .Without(x => x.DeletedAt)
                .Without(x => x.DeletedBy)
                .Without(x => x.Resume)
                .With(x => x.Value, FixtureBuilder.Faker.Phone.PhoneNumber())
                .With(x => x.ContactType, fixture.Create<ContactTypeEntity>())
                .OmitAutoProperties()
        );
    }
}