using AutoFixture;
using Bogus;
using JobMagnet.Infrastructure.Entities;
using JobMagnet.Models.Portfolio;
using JobMagnet.Models.Skill;

namespace JobMagnet.Integration.Tests.Fixtures.Customizations;

public class SkillItemCustomization : ICustomization
{
    private static int _autoIncrementId = 1;
    private readonly Faker _faker = new();

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

    private void ApplyCommonProperties(dynamic item)
    {
        item.Name = _faker.Company.CompanyName();
        item.ProficiencyLevel = _faker.Random.Int(1, 10);
        item.Category = _faker.Music.Genre();
        item.Rank = _faker.Random.Int(1, 10);
        item.IconUrl = _faker.Image.PicsumUrl();
    }
}