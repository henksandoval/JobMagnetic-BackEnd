using AutoFixture;
using Bogus;
using JobMagnet.Application.Contracts.Responses.Base;
using JobMagnet.Application.UseCases.CvParser.DTO.RawDTOs;
using JobMagnet.Domain.Core.Entities;

namespace JobMagnet.Shared.Tests.Fixtures.Customizations;

public class SkillCustomization : ICustomization
{
    private static readonly Faker Faker = new ();

    public void Customize(IFixture fixture)
    {
        fixture.Customize<SkillEntity>(composer =>
            composer.FromFactory((SkillSetEntity parentSkillSet) => BuildSkillEntity(parentSkillSet))
        );

        fixture.Customize<SkillSetEntity>(composer =>
            composer.FromFactory((long profileId) => BuildSkillSetEntity(profileId))
        );

        fixture.Register(() =>
            new SkillRaw(
                Faker.Lorem.Sentence(),
                []
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

    private static SkillSetEntity BuildSkillSetEntity(long profileId)
    {
        var skillSet = new SkillSetEntity(Faker.Lorem.Paragraph(), profileId);
        return skillSet;
    }

    private static SkillEntity BuildSkillEntity(SkillSetEntity parentSkillSet)
    {
        var skill = new SkillEntity(
            name: Faker.Name.JobTitle(),
            iconUrl: Faker.Image.PicsumUrl(),
            category: Faker.Commerce.Department(),
            skillSet: parentSkillSet,
            proficiencyLevel: (ushort)Faker.Random.Number(1, 10),
            rank: (ushort)Faker.Random.Number(1, 100)
        );

        return skill;
    }
}