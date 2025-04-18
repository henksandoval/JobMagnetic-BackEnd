using AutoFixture;
using Bogus;
using JobMagnet.Infrastructure.Entities;
using JobMagnet.Models.Skill;

namespace JobMagnet.Shared.Tests.Fixtures.Customizations;

public class SkillItemCustomization : ICustomization
{
    public void Customize(IFixture fixture)
    {
        fixture.Customize<SkillItemEntity>(composer =>
            composer
                .Without(x => x.Id)
                .Do(ApplyCommonProperties)
                .OmitAutoProperties());

        fixture.Customize<SkillItemRequest>(composer =>
            composer
                .Do(ApplyCommonProperties)
                .OmitAutoProperties());
    }

    private static void ApplyCommonProperties(dynamic item)
    {
        item.Name = FixtureBuilder.Faker.Company.CompanyName();
        item.ProficiencyLevel = FixtureBuilder.Faker.Random.UShort(1, 10);
        item.Category = FixtureBuilder.Faker.Music.Genre();
        item.Rank = FixtureBuilder.Faker.Random.UShort(1, 10);
        item.IconUrl = FixtureBuilder.Faker.Image.PicsumUrl();
    }
}