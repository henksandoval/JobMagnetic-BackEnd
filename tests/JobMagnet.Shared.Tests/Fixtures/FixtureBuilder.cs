using AutoFixture;
using Bogus;
using JobMagnet.Shared.Tests.Fixtures.Customizations;

namespace JobMagnet.Shared.Tests.Fixtures;

public static class FixtureBuilder
{
    public static readonly Faker Faker = new();

    public static IFixture Build()
    {
        var fixture = new Fixture();
        fixture.Customize(new ContactTypeCustomization());
        fixture.Customize(new SummaryCustomization());
        fixture.Customize(new PortfolioGalleryItemCustomization());
        fixture.Customize(new SkillItemCustomization());
        fixture.Customize(new ServiceGalleryItemCustomization());
        fixture.Customize(new EducationCustomization());
        fixture.Customize(new WorkExperienceCustomization());
        fixture.Customize(new TalentCustomization());
        fixture.Customize(new ProfileCustomization());
        fixture.Customize(new ServiceCustomization());
        fixture.Register(() => DateOnly.FromDateTime(Faker.Date.Past(30)));
        fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList().ForEach(b => fixture.Behaviors.Remove(b));
        fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        return fixture;
    }
}