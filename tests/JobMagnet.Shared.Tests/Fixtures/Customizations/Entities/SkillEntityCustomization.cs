using AutoFixture;
using JobMagnet.Application.Contracts.Responses.Base;
using JobMagnet.Domain.Core.Entities;

namespace JobMagnet.Shared.Tests.Fixtures.Customizations.Entities;

public class SkillEntityCustomization : ICustomization
{
    public void Customize(IFixture fixture)
    {
        fixture.Customize<SkillItemBase>(composer =>
            composer
                .With(x => x.Id, 0)
                .WithAutoProperties()
        );

        fixture.Customize<SkillBase>(composer =>
            composer.WithAutoProperties()
        );

        fixture.Customize<SkillEntity>(composer =>
            composer
                .With(x => x.Id, 0)
                .With(x => x.IsDeleted, false)
                .Without(x => x.DeletedAt)
                .Without(x => x.DeletedBy)
                .Without(x => x.Profile)
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