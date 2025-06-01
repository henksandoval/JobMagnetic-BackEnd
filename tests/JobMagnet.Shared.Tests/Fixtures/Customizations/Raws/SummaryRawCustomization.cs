using AutoFixture;
using JobMagnet.Application.UseCases.CvParser.DTO.RawDTOs;

namespace JobMagnet.Shared.Tests.Fixtures.Customizations.Raws;

public class SummaryRawCustomization : ICustomization
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