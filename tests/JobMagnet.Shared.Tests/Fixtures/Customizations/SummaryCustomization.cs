using AutoFixture;
using Bogus;
using JobMagnet.Application.UseCases.CvParser.DTO.RawDTOs;
using JobMagnet.Domain.Aggregates.Profiles;
using JobMagnet.Domain.Aggregates.Profiles.Entities;
using JobMagnet.Shared.Abstractions;
using JobMagnet.Shared.Tests.Abstractions;


namespace JobMagnet.Shared.Tests.Fixtures.Customizations;

public class SummaryCustomization : ICustomization
{
    private readonly IClock _clock = new DeterministicClock();
    private readonly IGuidGenerator _guidGenerator = new SequentialGuidGenerator();
    private static readonly Faker Faker = FixtureBuilder.Faker;

    public void Customize(IFixture fixture)
    {
        fixture.Customize<CareerHistory>(composer =>
            composer
                .FromFactory(() => CareerHistory.CreateInstance(
                    _guidGenerator,
                    _clock,
                    new ProfileId(),
                    Faker.Lorem.Paragraph()))
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