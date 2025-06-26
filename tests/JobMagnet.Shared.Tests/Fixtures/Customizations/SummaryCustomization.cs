using AutoFixture;
using Bogus;
using JobMagnet.Application.UseCases.CvParser.DTO.RawDTOs;
using JobMagnet.Domain.Aggregates.Profiles;
using JobMagnet.Domain.Aggregates.Profiles.Entities;


namespace JobMagnet.Shared.Tests.Fixtures.Customizations;

public class SummaryCustomization : ICustomization
{
    private static readonly Faker Faker = FixtureBuilder.Faker;

    public void Customize(IFixture fixture)
    {
        fixture.Customize<CareerHistory>(composer =>
            composer
                .FromFactory(() => new CareerHistory(
                    new CareerHistoryId(),
                    Guid.Empty,
                    Faker.Lorem.Paragraph(),
                    new ProfileId()
                ))
                .OmitAutoProperties()
        );

        fixture.Register(() =>
            new SummaryRaw(
                Faker.Lorem.Paragraph(),
                [],
                []
            )
        );
    }
}