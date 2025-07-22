using AutoFixture;
using Bogus;
using JobMagnet.Application.UseCases.CvParser.DTO.RawDTOs;
using JobMagnet.Domain.Aggregates.Profiles.Entities;
using JobMagnet.Domain.Aggregates.Profiles.ValueObjects;
using JobMagnet.Shared.Abstractions;
using JobMagnet.Shared.Tests.Abstractions;


namespace JobMagnet.Shared.Tests.Fixtures.Customizations;

public class TalentCustomization : ICustomization
{
    private static readonly Faker Faker = FixtureBuilder.Faker;
    private readonly IGuidGenerator _guidGenerator = new SequentialGuidGenerator();

    public void Customize(IFixture fixture)
    {
        fixture.Customize<Talent>(composer =>
            composer
                .FromFactory(() => 
                {
                    var description = Faker.PickRandom(StaticCustomizations.Talents);
                    return Talent.CreateInstance(
                        _guidGenerator, 
                        new ProfileId(),
                        description);
                })
                .OmitAutoProperties()
        );

        fixture.Customize<TalentRaw>(composer =>
            composer.FromFactory(() => new TalentRaw(
                Faker.PickRandom(StaticCustomizations.Talents)
            ))
        );
    }
}