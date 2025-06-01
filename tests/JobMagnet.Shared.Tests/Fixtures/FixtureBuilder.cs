using AutoFixture;
using Bogus;
using JobMagnet.Shared.Tests.Fixtures.Customizations.Entities;
using JobMagnet.Shared.Tests.Fixtures.Customizations.Raws;

namespace JobMagnet.Shared.Tests.Fixtures;

public static class FixtureBuilder
{
    public static readonly Faker Faker = new();

    public static IFixture Build()
    {
        var fixture = new Fixture();
        fixture
            .RegisterEntityCustomizations()
            .RegisterParseDtoCustomizations()
            .Register(() => DateOnly.FromDateTime(Faker.Date.Past(30)));

        fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList().ForEach(b => fixture.Behaviors.Remove(b));
        fixture.Behaviors.Add(new OmitOnRecursionBehavior());

        return fixture;
    }

    private static Fixture RegisterEntityCustomizations(this Fixture fixture)
    {
        fixture
            .Customize(new ProfileEntityCustomization())
            .Customize(new ContactTypeEntityCustomization())
            .Customize(new SummaryEntityCustomization())
            .Customize(new SkillItemEntityCustomization())
            .Customize(new ServiceGalleryItemEntityCustomization())
            .Customize(new EducationEntityCustomization())
            .Customize(new WorkExperienceEntityCustomization())
            .Customize(new TalentEntityCustomization())
            .Customize(new ServiceEntityCustomization())
            .Customize(new PortfolioEntityCustomization())
            .Customize(new SkillEntityCustomization())
            .Customize(new TestimonialEntityCustomization())
            .Customize(new ResumeEntityCustomization())
            .Customize(new ContactInfoEntityCustomization());

        return fixture;
    }

    private static Fixture RegisterParseDtoCustomizations(this Fixture fixture)
    {
        fixture
            .Customize(new ProfileRawCustomization())
            .Customize(new SummaryRawCustomization())
            .Customize(new SkillRawCustomization())
            .Customize(new SkillDetailRawCustomization())
            .Customize(new ServiceGalleryRawCustomization())
            .Customize(new EducationRawCustomization())
            .Customize(new WorkExperienceRawCustomization())
            .Customize(new TalentRawCustomization())
            .Customize(new ServiceRawCustomization())
            .Customize(new PortfolioRawCustomization())
            .Customize(new TestimonialRawCustomization())
            .Customize(new ResumeRawCustomization())
            .Customize(new ContactInfoRawCustomization());

        return fixture;
    }
}