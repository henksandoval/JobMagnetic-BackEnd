using AutoFixture;
using Bogus;
using JobMagnet.Application.Contracts.Responses.Base;
using JobMagnet.Application.UseCases.CvParser.DTO.RawDTOs;
using JobMagnet.Domain.Core.Entities.Skills;
using JobMagnet.Infrastructure.Persistence.Seeders.Collections;

namespace JobMagnet.Shared.Tests.Fixtures.Customizations;

public class SkillCustomization : ICustomization
{
    private static readonly Faker Faker = FixtureBuilder.Faker;

    public void Customize(IFixture fixture)
    {
        fixture.Customize<SkillType>(composer =>
            composer
                .FromFactory(() => Faker.PickRandom(new SkillTypesCollection().GetSkillTypesWithAliases().ToList()))
                .OmitAutoProperties()
        );

        fixture.Register(() => new SkillSetEntity(Faker.Lorem.Sentence(), 0));

        fixture.Register(() => new SkillSetRaw(Faker.Lorem.Sentence(),[]));

        fixture.Register(() =>
            new SkillRaw(
                Faker.PickRandom(StaticCustomizations.Skills),
                    Faker.Random.Int(1, 10).ToString()
            )
        );

        fixture.Customize<SkillItemBase>(composer =>
            composer
                .With(x => x.Id, 0)
                .WithAutoProperties()
        );

        fixture.Customize<SkillBase>(composer =>
            composer.WithAutoProperties()
        );
    }
}