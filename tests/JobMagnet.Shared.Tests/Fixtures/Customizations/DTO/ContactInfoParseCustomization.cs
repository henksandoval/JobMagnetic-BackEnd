using AutoFixture;
using JobMagnet.Application.UseCases.CvParser.ParsingDTOs;
using JobMagnet.Domain.Entities;

namespace JobMagnet.Shared.Tests.Fixtures.Customizations.DTO;

public class ContactInfoParseCustomization : ICustomization
{
    public void Customize(IFixture fixture)
    {
        fixture.Customize<ContactInfoParseDto>(composer => composer
            .Do(ApplyCommonProperties)
            .OmitAutoProperties());
    }

    private static void ApplyCommonProperties(dynamic item)
    {
        item.Value = FixtureBuilder.Faker.Phone.PhoneNumber();
        item.ContactType = FixtureBuilder.Faker.PickRandom(StaticCustomizations.ContactTypes).Name;
    }
}