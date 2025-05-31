using AutoFixture;
using JobMagnet.Application.UseCases.CvParser.RawDTOs;

namespace JobMagnet.Shared.Tests.Fixtures.Customizations.Raws;

public class ContactInfoParseCustomization : ICustomization
{
    public void Customize(IFixture fixture)
    {
        fixture.Customize<ContactInfoRaw>(composer => composer
            .Do(ApplyCommonProperties)
            .OmitAutoProperties());
    }

    private static void ApplyCommonProperties(dynamic item)
    {
        item.Value = FixtureBuilder.Faker.Phone.PhoneNumber();
        item.ContactType = FixtureBuilder.Faker.PickRandom(StaticCustomizations.ContactTypes).Name;
    }
}