using AutoFixture;
using JobMagnet.Application.UseCases.CvParser.DTO.RawDTOs;
using JobMagnet.Shared.Tests.Utils;

namespace JobMagnet.Shared.Tests.Fixtures.Customizations.Raws;

public class ResumeParseCustomization : ICustomization
{
    public void Customize(IFixture fixture)
    {
        fixture.Customize<ResumeRaw>(composer =>
            composer
                .Without(x => x.ContactInfo)
                .Do(ApplyCommonProperties)
                .OmitAutoProperties()
        );
    }

    private static void ApplyCommonProperties(dynamic item)
    {
        item.JobTitle = FixtureBuilder.Faker.Name.JobTitle();
        item.About = FixtureBuilder.Faker.Lorem.Paragraph();
        item.Address = FixtureBuilder.Faker.Address.FullAddress();
        item.Summary = FixtureBuilder.Faker.Lorem.Paragraph();
        item.Overview = FixtureBuilder.Faker.Lorem.Paragraph();
        item.Title = TestUtilities.OptionalValue(FixtureBuilder.Faker, f => f.Name.Prefix());
        item.Suffix = TestUtilities.OptionalValue(FixtureBuilder.Faker, f => f.Name.Suffix());
    }
}