using AutoFixture;
using JobMagnet.Application.UseCases.CvParser.ParsingDTOs;
using JobMagnet.Domain.Entities;

namespace JobMagnet.Shared.Tests.Fixtures.Customizations.DTO;

public class TalentParseCustomization : ICustomization
{
    public void Customize(IFixture fixture)
    {
        fixture.Customize<TalentParseDto>(composer =>
            composer
                .Do(ApplyCommonProperties)
                .OmitAutoProperties());
    }

    private static void ApplyCommonProperties(dynamic item)
    {
        item.Description = FixtureBuilder.Faker.PickRandom(StaticCustomizations.Talents);
    }
}