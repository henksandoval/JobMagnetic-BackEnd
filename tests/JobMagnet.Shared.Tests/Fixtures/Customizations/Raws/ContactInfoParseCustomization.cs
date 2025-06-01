using AutoFixture;
using JobMagnet.Application.UseCases.CvParser.DTO.RawDTOs;

namespace JobMagnet.Shared.Tests.Fixtures.Customizations.Raws;

public class ContactInfoParseCustomization : ICustomization
{
    public void Customize(IFixture fixture)
    {
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