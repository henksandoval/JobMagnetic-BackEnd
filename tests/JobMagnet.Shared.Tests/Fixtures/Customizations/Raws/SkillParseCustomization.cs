using AutoFixture;
using JobMagnet.Application.UseCases.CvParser.DTO.RawDTOs;

namespace JobMagnet.Shared.Tests.Fixtures.Customizations.Raws;

public class SkillParseCustomization : ICustomization
{
    public void Customize(IFixture fixture)
    {
        fixture.Register<SkillRaw>(() =>
            new(
                FixtureBuilder.Faker.Lorem.Sentence(),
                []
            )
        );
    }
}