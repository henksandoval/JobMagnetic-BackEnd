using AutoFixture;
using Bogus;
using JobMagnet.Application.UseCases.CvParser.DTO.RawDTOs;
using JobMagnet.Domain.Aggregates.Profiles;
using JobMagnet.Domain.Aggregates.Profiles.Entities;


namespace JobMagnet.Shared.Tests.Fixtures.Customizations;

public class TalentCustomization : ICustomization
{
    private static readonly Faker Faker = FixtureBuilder.Faker;
    public void Customize(IFixture fixture)
    {
        fixture.Customize<Talent>(composer =>
            composer
                .FromFactory(() => new Talent(
                    Faker.PickRandom(StaticCustomizations.Talents),
                    new ProfileId(),
                    new TalentId()
            )).OmitAutoProperties()
        );

        fixture.Customize<TalentRaw>(composer =>
            composer.FromFactory(() => new TalentRaw(
                Faker.PickRandom(StaticCustomizations.Talents)
            ))
        );
    }
}