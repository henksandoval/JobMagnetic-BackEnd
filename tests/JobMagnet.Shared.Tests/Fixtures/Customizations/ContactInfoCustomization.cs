using AutoFixture;
using JobMagnet.Application.UseCases.CvParser.DTO.RawDTOs;
using JobMagnet.Domain.Core.Entities;

namespace JobMagnet.Shared.Tests.Fixtures.Customizations;

public class ContactInfoCustomization : ICustomization
{
    public void Customize(IFixture fixture)
    {
        fixture.Customize<ContactInfoEntity>(composer => composer
            .With(x => x.Id, 0)
            .With(x => x.IsDeleted, false)
            .Without(x => x.DeletedAt)
            .Without(x => x.DeletedBy)
            .Without(x => x.Resume)
            .OmitAutoProperties());

        fixture.Customize<ContactInfoRaw>(composer =>
            composer.FromFactory(() =>
                new ContactInfoRaw(
                    FixtureBuilder.Faker.PickRandom(StaticCustomizations.ContactTypes).Name,
                    FixtureBuilder.Faker.Phone.PhoneNumber()
                )
            )
        );
    }
}