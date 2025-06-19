using AutoFixture;
using Bogus;
using JobMagnet.Application.Contracts.Responses.Base;
using JobMagnet.Application.UseCases.CvParser.DTO.RawDTOs;
using JobMagnet.Domain.Core.Entities;

namespace JobMagnet.Shared.Tests.Fixtures.Customizations;

public class SkillCustomization : ICustomization
{
    private static readonly Faker Faker = new();

    public void Customize(IFixture fixture)
    {
        fixture.Customize<SkillEntity>(composer =>
            composer
                .FromFactory((SkillSetEntity parentSkillSet) => BuildSkillEntity(parentSkillSet))
                .OmitAutoProperties()
        );

        fixture.Customize<SkillSetEntity>(composer =>
            composer
                .FromFactory((long profileId) => BuildSkillSetEntity(profileId))
                .OmitAutoProperties()
        );

        fixture.Register(() =>
            new SkillSetRaw(
                Faker.Lorem.Sentence(),
                []
            )
        );

        fixture.Register(() =>
            new SkillRaw(
                FixtureBuilder.Faker.PickRandom(StaticCustomizations.Skills),
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

    private static SkillSetEntity BuildSkillSetEntity(long profileId)
    {
        var skillSet = new SkillSetEntity(Faker.Lorem.Paragraph(), profileId);
        return skillSet;
    }

    private static SkillEntity BuildSkillEntity(SkillSetEntity parentSkillSet)
    {
        var randomSkillTypeId = Faker.Random.Short(1, 5);

        var (type, uri, categoryName) = GenerateSkillTypes(randomSkillTypeId);

        var category = new SkillCategory(categoryName);
        var skillType = new SkillType(0, type, category, new Uri(uri));
        parentSkillSet.AddSkill((ushort)Faker.Random.Number(1, 10), skillType);

        return parentSkillSet.Skills.Last();
    }

    private static (string type, string uri, string category) GenerateSkillTypes(short contactTypeId)
    {
        return contactTypeId switch
        {
            1 => ("HTML", "https://cdn.simpleicons.org/html5", "Software Development"),
            2 => ("CSS", "https://cdn.simpleicons.org/css3", "Software Development"),
            3 => ("JavaScript", "https://cdn.simpleicons.org/javascript", "Software Development"),
            4 => ("C#", "https://cdn.simpleicons.org/dotnet", "Software Development"),
            5 => ("TS", "https://cdn.simpleicons.org/typescript", "Software Development"),
            _ => throw new ArgumentOutOfRangeException(nameof(contactTypeId), contactTypeId, null)
        };
    }
}