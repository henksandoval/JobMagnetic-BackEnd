using AutoFixture;
using JobMagnet.Application.Contracts.Responses.Base;
using JobMagnet.Application.UseCases.CvParser.DTO.RawDTOs;
using JobMagnet.Domain.Core.Entities;

namespace JobMagnet.Shared.Tests.Fixtures.Customizations;

public class SkillDetailCustomization : ICustomization
{
    public void Customize(IFixture fixture)
    {
        fixture.Customize<SkillItemEntity>(composer =>
            composer
                .Without(x => x.Id)
                .Do(ApplyCommonProperties)
                .OmitAutoProperties());

        fixture.Customize<SkillItemBase>(composer =>
            composer
                .Do(ApplyCommonProperties)
                .OmitAutoProperties());

        fixture.Customize<SkillDetailRaw>(composer =>
            composer.FromFactory(() =>
                new SkillDetailRaw(
                    FixtureBuilder.Faker.Company.CompanyName(),
                    FixtureBuilder.Faker.Random.UShort(1, 10).ToString()
                )
            )
        );
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