using AutoFixture;
using Bogus;
using JobMagnet.Application.Contracts.Responses.Base;
using JobMagnet.Application.UseCases.CvParser.DTO.RawDTOs;

namespace JobMagnet.Shared.Tests.Fixtures.Customizations;

public class SkillDetailCustomization : ICustomization
{
    private static readonly Faker Faker = FixtureBuilder.Faker;

    public void Customize(IFixture fixture)
    {
        fixture.Customize<SkillBase>(composer =>
            composer
                .Do(ApplyCommonProperties)
                .OmitAutoProperties());

        fixture.Customize<SkillRaw>(composer =>
            composer.FromFactory(() =>
                new SkillRaw(
                    Faker.Company.CompanyName(),
                    Faker.Random.UShort(1, 10).ToString()
                )
            )
        );
    }

    private static void ApplyCommonProperties(dynamic item)
    {
        item.Name = Faker.Company.CompanyName();
        item.ProficiencyLevel = Faker.Random.UShort(1, 10);
        item.Category = Faker.Music.Genre();
        item.Rank = Faker.Random.UShort(1, 10);
        item.IconUrl = Faker.Image.PicsumUrl();
    }
}