using AutoFixture;
using JobMagnet.Application.UseCases.CvParser.RawDTOs;

namespace JobMagnet.Shared.Tests.Fixtures.Customizations.Raws;

public class SummaryParseCustomization : ICustomization
{
    public void Customize(IFixture fixture)
    {
        fixture.Customize<SummaryRaw>(composer =>
            composer
                .Do(ApplyCommonProperties)
                .OmitAutoProperties()
        );
    }

    private static void ApplyCommonProperties(dynamic item)
    {
        item.Introduction = FixtureBuilder.Faker.Lorem.Paragraph();
        item.Education = FixtureBuilder.Build().CreateMany<EducationRaw>().ToList();
        item.WorkExperiences = FixtureBuilder.Build().CreateMany<WorkExperienceRaw>().ToList();
    }
}