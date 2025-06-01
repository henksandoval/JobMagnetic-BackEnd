using AutoFixture;
using JobMagnet.Application.UseCases.CvParser.DTO.RawDTOs;

namespace JobMagnet.Shared.Tests.Fixtures.Customizations.Raws;

public class TalentParseCustomization : ICustomization
{
    public void Customize(IFixture fixture)
    {
        fixture.Customize<TalentRaw>(composer =>
            composer.FromFactory(() => new TalentRaw(
                FixtureBuilder.Faker.PickRandom(StaticCustomizations.Talents)
            ))
        );
    }
}