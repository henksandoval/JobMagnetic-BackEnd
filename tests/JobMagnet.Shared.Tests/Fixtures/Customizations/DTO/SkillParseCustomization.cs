using AutoFixture;
using JobMagnet.Application.UseCases.CvParser.ParsingDTOs;

namespace JobMagnet.Shared.Tests.Fixtures.Customizations.DTO;

public class SkillParseCustomization : ICustomization
{
    public void Customize(IFixture fixture)
    {
        fixture.Customize<SkillParseDto>(composer =>
            composer
                .Do(ApplyCommonProperties)
                .OmitAutoProperties()
        );
    }

    private static void ApplyCommonProperties(dynamic item)
    {
        item.Overview = FixtureBuilder.Faker.Lorem.Sentence();
        item.SkillDetails = FixtureBuilder.Build().CreateMany<SkillDetailParseDto>().ToList();
    }
}