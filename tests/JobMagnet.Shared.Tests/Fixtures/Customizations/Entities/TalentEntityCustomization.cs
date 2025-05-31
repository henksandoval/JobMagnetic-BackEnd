using AutoFixture;
using JobMagnet.Domain.Core.Entities;

namespace JobMagnet.Shared.Tests.Fixtures.Customizations.Entities;

public class TalentEntityCustomization : ICustomization
{
    public void Customize(IFixture fixture)
    {
        fixture.Customize<TalentEntity>(composer =>
            composer
                .With(x => x.Id, 0)
                .With(x => x.IsDeleted, false)
                .Without(x => x.DeletedAt)
                .Without(x => x.DeletedBy)
                .Do(ApplyCommonProperties)
                .OmitAutoProperties());
    }

    private static void ApplyCommonProperties(dynamic item)
    {
        item.Description = FixtureBuilder.Faker.PickRandom(StaticCustomizations.Talents);
    }
}