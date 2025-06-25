using AutoFixture;
using JobMagnet.Application.UseCases.CvParser.DTO.RawDTOs;
using JobMagnet.Domain.Core.Entities;

namespace JobMagnet.Shared.Tests.Fixtures.Customizations;

public class TalentCustomization : ICustomization
{
    public void Customize(IFixture fixture)
    {
        fixture.Customize<TalentEntity>(composer =>
            composer
                .FromFactory(() => new TalentEntity(
                    FixtureBuilder.Faker.PickRandom(StaticCustomizations.Talents)
            )).OmitAutoProperties()
        );

        fixture.Customize<TalentRaw>(composer =>
            composer.FromFactory(() => new TalentRaw(
                FixtureBuilder.Faker.PickRandom(StaticCustomizations.Talents)
            ))
        );
    }
}