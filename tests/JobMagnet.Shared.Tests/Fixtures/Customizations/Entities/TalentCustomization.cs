using AutoFixture;
using JobMagnet.Domain.Entities;

namespace JobMagnet.Shared.Tests.Fixtures.Customizations;

public class TalentCustomization : ICustomization
{
    private static readonly string[] Talens =
    [
        "Creative",
        "Problem Solver",
        "Team Player",
        "Adaptable",
        "Detail-Oriented",
        "Strong Communicator",
        "Analytical"
    ];

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
        item.Description = FixtureBuilder.Faker.PickRandom(Talens);
    }
}