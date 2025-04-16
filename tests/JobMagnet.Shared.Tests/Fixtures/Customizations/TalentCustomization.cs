using AutoFixture;
using Bogus;
using JobMagnet.Infrastructure.Entities;

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

    private readonly Faker _faker = new();

    public void Customize(IFixture fixture)
    {
        fixture.Customize<TalentEntity>(composer =>
            composer
                .Without(x => x.Id)
                .Do(ApplyCommonProperties)
                .OmitAutoProperties());
    }

    private void ApplyCommonProperties(dynamic item)
    {
        item.Description = _faker.PickRandom(Talens);
    }
}