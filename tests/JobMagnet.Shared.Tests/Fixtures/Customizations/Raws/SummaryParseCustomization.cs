using AutoFixture;
using JobMagnet.Application.UseCases.CvParser.DTO.RawDTOs;

namespace JobMagnet.Shared.Tests.Fixtures.Customizations.Raws;

public class SummaryParseCustomization : ICustomization
{
    public void Customize(IFixture fixture)
    {
        fixture.Register<SummaryRaw>(() =>
            new (
                FixtureBuilder.Faker.Lorem.Paragraph(),
                [],
                []
            )
        );
    }
}