using AutoFixture;
using JobMagnet.Application.UseCases.CvParser.ParsingDTOs;

namespace JobMagnet.Shared.Tests.Fixtures.Customizations.DTO;

public class SummaryParseCustomization : ICustomization
{
    public void Customize(IFixture fixture)
    {
        fixture.Customize<SummaryParseDto>(composer =>
            composer
                .Do(ApplyCommonProperties)
                .OmitAutoProperties()
        );
    }

    private static void ApplyCommonProperties(dynamic item)
    {
        item.Introduction = FixtureBuilder.Faker.Lorem.Paragraph();
        item.Education = FixtureBuilder.Build().CreateMany<EducationParseDto>().ToList();
        item.WorkExperiences = FixtureBuilder.Build().CreateMany<WorkExperienceParseDto>().ToList();
    }
}