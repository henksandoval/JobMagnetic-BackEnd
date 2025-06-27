using AutoFixture;
using Bogus;
using JobMagnet.Application.Contracts.Responses.Base;
using JobMagnet.Application.UseCases.CvParser.DTO.RawDTOs;
using JobMagnet.Domain.Aggregates.Profiles;
using JobMagnet.Domain.Aggregates.Profiles.Entities;
using JobMagnet.Domain.Aggregates.Skills.Entities;
using JobMagnet.Infrastructure.Persistence.Seeders.Collections;
using JobMagnet.Shared.Abstractions;
using JobMagnet.Shared.Tests.Abstractions;
using JobMagnet.Shared.Tests.Factories;

namespace JobMagnet.Shared.Tests.Fixtures.Customizations;

public class SkillCustomization : ICustomization
{
    private readonly IClock _clock = new DeterministicClock();
    private readonly IGuidGenerator _guidGenerator = new SequentialGuidGenerator();
    private static readonly Faker Faker = FixtureBuilder.Faker;

    public void Customize(IFixture fixture)
    {
        var factory = new TestDataFactory();

        fixture.Customize<SkillType>(composer =>
            composer
                .FromFactory(() => Faker.PickRandom(factory.PredefinedSkillTypes.ToList()))
                .OmitAutoProperties()
        );

        fixture.Register(() => SkillSet.CreateInstance(
            _guidGenerator,
            _clock,
            new ProfileId(),
            Faker.Lorem.Sentence()));

        fixture.Register(() => new SkillSetRaw(Faker.Lorem.Sentence(), []));

        fixture.Register(() =>
            new SkillRaw(
                Faker.PickRandom(StaticCustomizations.Skills),
                Faker.Random.Int(1, 10).ToString()
            )
        );

        fixture.Customize<SkillItemBase>(composer =>
            composer
                .With(x => x.Id, Faker.Random.Guid())
                .WithAutoProperties()
        );

        fixture.Customize<SkillBase>(composer =>
            composer.WithAutoProperties()
        );
    }
}