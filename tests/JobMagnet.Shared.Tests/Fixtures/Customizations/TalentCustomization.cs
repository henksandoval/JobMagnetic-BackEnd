using AutoFixture;
using Bogus;
using JobMagnet.Application.UseCases.CvParser.DTO.RawDTOs;
using JobMagnet.Domain.Aggregates.Profiles;
using JobMagnet.Domain.Aggregates.Profiles.Entities;
using JobMagnet.Shared.Abstractions;
using JobMagnet.Shared.Tests.Abstractions;


namespace JobMagnet.Shared.Tests.Fixtures.Customizations;

public class TalentCustomization : ICustomization
{
    private readonly IClock _clock = new DeterministicClock();
    private readonly IGuidGenerator _guidGenerator = new SequentialGuidGenerator();
    private static readonly Faker Faker = FixtureBuilder.Faker;

    public void Customize(IFixture fixture)
    {
        fixture.Customize<Talent>(composer =>
            composer
                .FromFactory(() => Talent.CreateInstance(
                    _guidGenerator,
                    _clock,
                    new ProfileId(),
                    Faker.PickRandom(StaticCustomizations.Talents)
                )).OmitAutoProperties()
        );

        fixture.Customize<TalentRaw>(composer =>
            composer.FromFactory(() => new TalentRaw(
                Faker.PickRandom(StaticCustomizations.Talents)
            ))
        );
    }
}