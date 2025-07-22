using AutoFixture;
using Bogus;
using JobMagnet.Application.Contracts.Responses.Base;
using JobMagnet.Application.UseCases.CvParser.DTO.RawDTOs;
using JobMagnet.Domain.Aggregates.Profiles.Entities;
using JobMagnet.Domain.Aggregates.Profiles.ValueObjects;
using JobMagnet.Domain.Aggregates.SkillTypes;
using JobMagnet.Shared.Abstractions;
using JobMagnet.Shared.Tests.Abstractions;
using JobMagnet.Shared.Tests.Factories;

namespace JobMagnet.Shared.Tests.Fixtures.Customizations;

public class SkillCustomization : ICustomization
{
    private static readonly Faker Faker = FixtureBuilder.Faker;
    private readonly IGuidGenerator _guidGenerator = new SequentialGuidGenerator();

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
            new ProfileId(),
            Faker.Lorem.Sentence()));

        fixture.Register(() => new SkillSetRaw(Faker.Lorem.Sentence(), []));

        fixture.Register(() =>
            new SkillRaw(
                Faker.PickRandom(StaticCustomizations.Skills),
                Faker.Random.Int(1, 10).ToString()
            )
        );

        fixture.Customize<SkillBase>(composer =>
            composer
                .With(x => x.Name, () => Faker.PickRandom(StaticCustomizations.Skills))
                .With(x => x.ProficiencyLevel, () => Faker.Random.Int(0, 10))
                .WithAutoProperties()
        );

        fixture.Customize<SkillSetBase>(composer =>
            composer.With(x => x.Overview, Faker.Lorem.Sentence())
        );
    }
}