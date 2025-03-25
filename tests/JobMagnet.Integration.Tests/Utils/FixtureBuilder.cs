using AutoFixture;
using JobMagnet.Infrastructure.Entities;

namespace JobMagnet.Integration.Tests.Utils;

public static class FixtureBuilder
{
    public static IFixture Build()
    {
        var fixture = new Fixture();
        fixture.Register(() => DateOnly.FromDateTime(DateTime.Now.AddYears(-new Random().Next(18, 60))));
        fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList().ForEach(b => fixture.Behaviors.Remove(b));
        fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        return fixture;
    }

    public static ResumeEntity BuildResumeEntity(this IFixture fixture)
    {
        var entity = fixture.Build<ResumeEntity>()
            .With(x => x.Id, 0)
            .With(x => x.IsDeleted, false)
            .Without(x => x.DeletedAt)
            .Without(x => x.DeletedAt)
            .Create();
        return entity;
    }
}