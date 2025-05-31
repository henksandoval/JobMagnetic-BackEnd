using AutoFixture;
using JobMagnet.Application.UseCases.CvParser.RawDTOs;

namespace JobMagnet.Shared.Tests.Fixtures.Customizations.Raws;

public class SkillDetailParseCustomization : ICustomization
{
    public void Customize(IFixture fixture)
    {
        fixture.Customize<SkillDetailRaw>(composer =>
            composer
                .Do(ApplyCommonProperties)
                .OmitAutoProperties());
    }

    private static void ApplyCommonProperties(dynamic item)
    {
        item.Name = FixtureBuilder.Faker.Company.CompanyName();
        item.Level = FixtureBuilder.Faker.Random.UShort(1, 10).ToString();
    }
}