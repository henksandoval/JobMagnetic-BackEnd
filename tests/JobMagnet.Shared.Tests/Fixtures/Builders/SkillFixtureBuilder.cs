using AutoFixture;
using JobMagnet.Infrastructure.Entities;

namespace JobMagnet.Shared.Tests.Fixtures.Builders;

public static class SkillFixtureBuilder
{
    public static SkillEntity BuildSkillEntity(this IFixture fixture, int skillItems = 5)
    {
        var skillDetailItems = fixture.CreateMany<SkillItemEntity>(skillItems).ToList();
        var skillEntity = fixture.Build<SkillEntity>()
            .With(x => x.Id, 0)
            .With(x => x.IsDeleted, false)
            .Without(x => x.DeletedAt)
            .Without(x => x.DeletedBy)
            .With(x => x.Profile, fixture.GetProfileEntityComposer().Create())
            .With(x => x.SkillDetails, skillDetailItems)
            .Create();

        return skillEntity;
    }
}