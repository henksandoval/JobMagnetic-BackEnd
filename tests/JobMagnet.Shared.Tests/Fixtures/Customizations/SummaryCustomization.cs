using AutoFixture;
using JobMagnet.Application.UseCases.CvParser.DTO.RawDTOs;
using JobMagnet.Domain.Core.Entities;

namespace JobMagnet.Shared.Tests.Fixtures.Customizations;

public class SummaryCustomization : ICustomization
{
    public void Customize(IFixture fixture)
    {
        fixture.Customize<SummaryEntity>(composer =>
            composer
                .FromFactory(() => new SummaryEntity(
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