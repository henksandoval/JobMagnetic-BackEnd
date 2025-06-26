using AutoFixture;
using JobMagnet.Application.UseCases.CvParser.DTO.RawDTOs;
using JobMagnet.Domain.Core.Entities;

namespace JobMagnet.Shared.Tests.Fixtures.Customizations;

public class SummaryCustomization : ICustomization
{
    public void Customize(IFixture fixture)
    {
        fixture.Customize<CareerHistory>(composer =>
            composer
                .FromFactory(() => new CareerHistory(
                    FixtureBuilder.Faker.Lorem.Paragraph()
                ))
                .OmitAutoProperties()
        );

        fixture.Register(() =>
            new SummaryRaw(
                FixtureBuilder.Faker.Lorem.Paragraph(),
                [],
                []
            )
        );
    }
}