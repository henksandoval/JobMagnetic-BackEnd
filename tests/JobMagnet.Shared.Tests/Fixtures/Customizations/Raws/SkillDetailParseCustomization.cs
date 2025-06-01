using AutoFixture;
using JobMagnet.Application.UseCases.CvParser.DTO.RawDTOs;

namespace JobMagnet.Shared.Tests.Fixtures.Customizations.Raws;

public class SkillDetailParseCustomization : ICustomization
{
    public void Customize(IFixture fixture)
    {
        fixture.Customize<SkillDetailRaw>(composer =>
            composer.FromFactory(() =>
                new SkillDetailRaw(
                    FixtureBuilder.Faker.Company.CompanyName(),
                    FixtureBuilder.Faker.Random.UShort(1, 10).ToString()
                )
            )
        );
    }
}