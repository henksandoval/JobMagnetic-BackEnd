using AutoFixture;
using JobMagnet.Application.Models.Base;
using JobMagnet.Application.UseCases.CvParser.ParsingDTOs;

namespace JobMagnet.Shared.Tests.Fixtures.Customizations.DTO;

public class SkillDetailParseCustomization : ICustomization
{
    public void Customize(IFixture fixture)
    {
        fixture.Customize<SkillDetailParseDto>(composer =>
            composer
                .Do(ApplyCommonProperties)
                .OmitAutoProperties());
    }

    private static void ApplyCommonProperties(dynamic item)
    {
        item.Name = FixtureBuilder.Faker.Company.CompanyName();
        item.Level = FixtureBuilder.Faker.Random.UShort(1, 10);
    }
}