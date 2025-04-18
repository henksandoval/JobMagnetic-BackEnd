using AutoFixture;
using Bogus;
using JobMagnet.Infrastructure.Entities;

namespace JobMagnet.Shared.Tests.Fixtures.Customizations;

public class SkillCustomization : ICustomization
{
    public void Customize(IFixture fixture)
    {
        fixture.Customize<SkillEntity>(composer =>
            composer
                .With(x => x.Id, 0)
                .With(x => x.IsDeleted, false)
                .Without(x => x.DeletedAt)
                .Without(x => x.DeletedBy)
                .With(x => x.Profile, fixture.Create<ProfileEntity>())
                .With(x => x.ProfileId, 0)
                .Do(ApplyCommonProperties)
                .OmitAutoProperties()
        );
    }

    private static void ApplyCommonProperties(dynamic item)
    {
        item.Overview = FixtureBuilder.Faker.Lorem.Sentence();
        item.SkillDetails = FixtureBuilder.Build().CreateMany<SkillItemEntity>().ToList();
    }
}