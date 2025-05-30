using AutoFixture;
using Bogus;
using JobMagnet.Shared.Tests.Fixtures.Customizations.Entities;

namespace JobMagnet.Shared.Tests.Fixtures;

public static class FixtureBuilder
{
    public static readonly Faker Faker = new();

    public static IFixture Build()
    {
        var fixture = new Fixture();
        fixture
            .RegisterEntityCustomizations()
            .Register(() => DateOnly.FromDateTime(Faker.Date.Past(30)));

        fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList().ForEach(b => fixture.Behaviors.Remove(b));
        fixture.Behaviors.Add(new OmitOnRecursionBehavior());

        return fixture;
    }

    private static Fixture RegisterEntityCustomizations(this Fixture fixture)
    {
        fixture
            .Customize(new ContactTypeEntityCustomization())
            .Customize(new SummaryEntityCustomization())
            .Customize(new SkillItemEntityCustomization())
            .Customize(new ServiceGalleryItemEntityCustomization())
            .Customize(new EducationEntityCustomization())
            .Customize(new WorkExperienceEntityCustomization())
            .Customize(new TalentEntityCustomization())
            .Customize(new ProfileEntityCustomization())
            .Customize(new ServiceEntityCustomization())
            .Customize(new PortfolioEntityCustomization())
            .Customize(new SkillEntityCustomization())
            .Customize(new TestimonialEntityCustomization())
            .Customize(new ResumeEntityCustomization())
            .Customize(new ContactInfoEntityCustomization());

        return fixture;
    }
}