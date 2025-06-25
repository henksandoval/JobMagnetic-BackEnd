using AutoFixture;
using Bogus;
using JobMagnet.Application.UseCases.CvParser.DTO.RawDTOs;
using JobMagnet.Domain.Core.Entities;

namespace JobMagnet.Shared.Tests.Fixtures.Customizations;

public class TalentCustomization : ICustomization
{
    private static readonly Faker Faker = FixtureBuilder.Faker;
    public void Customize(IFixture fixture)
    {
        fixture.Customize<TalentEntity>(composer =>
            composer
                .FromFactory(() => new TalentEntity(
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