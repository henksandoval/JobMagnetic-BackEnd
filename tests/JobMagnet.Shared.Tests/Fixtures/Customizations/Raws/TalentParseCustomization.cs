using AutoFixture;
using JobMagnet.Application.UseCases.CvParser.RawDTOs;

namespace JobMagnet.Shared.Tests.Fixtures.Customizations.Raws;

public class TalentParseCustomization : ICustomization
{
    public void Customize(IFixture fixture)
    {
        fixture.Customize<TalentRaw>(composer =>
            composer
                .Do(ApplyCommonProperties)
                .OmitAutoProperties());
    }

    private static void ApplyCommonProperties(dynamic item)
    {
        item.Description = FixtureBuilder.Faker.PickRandom(StaticCustomizations.Talents);
    }
}