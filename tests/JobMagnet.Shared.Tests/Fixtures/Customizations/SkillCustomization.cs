using AutoFixture;
using Bogus;
using JobMagnet.Application.Contracts.Responses.Base;
using JobMagnet.Application.UseCases.CvParser.DTO.RawDTOs;
using JobMagnet.Domain.Core.Entities;
using JobMagnet.Infrastructure.Persistence.Seeders.Collections;

namespace JobMagnet.Shared.Tests.Fixtures.Customizations;

public class SkillCustomization : ICustomization
{
    private static readonly Faker Faker = FixtureBuilder.Faker;
    private static readonly List<SkillType> Skills = [];

    static SkillCustomization()
    {
        var skillsFromCollection = new SkillTypesCollection().GetSkillTypesWithAliases();

        var currentId = 1;

        foreach (var skill in skillsFromCollection)
        {
            var testSkill = new SkillType(currentId++, skill.Name, skill.Category, new Uri(skill.IconUrl));

            foreach (var alias in skill.Aliases)
            {
                testSkill.AddAlias(alias.Alias);
            }

            Skills.Add(testSkill);
        }
    }

    public void Customize(IFixture fixture)
    {
        fixture.Customize<SkillType>(composer =>
            composer
                .FromFactory(() => Faker.PickRandom(Skills))
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