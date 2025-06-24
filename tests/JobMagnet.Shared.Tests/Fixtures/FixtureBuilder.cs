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
        fixture
            .RegisterCustomizations()
            .Register(() => DateOnly.FromDateTime(Faker.Date.Past(30)));

        fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList().ForEach(b => fixture.Behaviors.Remove(b));
        fixture.Behaviors.Add(new OmitOnRecursionBehavior());

        return fixture;
    }

    private static Fixture RegisterCustomizations(this Fixture fixture)
    {
        fixture
            .Customize(new ContactInfoCustomization())
            .Customize(new ContactTypeCustomization())
            .Customize(new EducationCustomization())
            .Customize(new ProjectCustomization())
            .Customize(new ProfileCustomization())
            .Customize(new ResumeCustomization())
            .Customize(new SummaryCustomization())
            .Customize(new SkillDetailCustomization())
            .Customize(new WorkExperienceCustomization())
            .Customize(new TalentCustomization())
            .Customize(new SkillCustomization())
            .Customize(new TestimonialCustomization());

        return fixture;
    }
}