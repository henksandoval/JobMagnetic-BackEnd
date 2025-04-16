using AutoFixture;
using AutoFixture.Dsl;
using Bogus;
using JobMagnet.Infrastructure.Entities;
using JobMagnet.Integration.Tests.Fixtures.Customizations;
using JobMagnet.Integration.Tests.Utils;

namespace JobMagnet.Integration.Tests.Fixtures;

public static class FixtureBuilder
{
    private static readonly Faker Faker = new();

    public static IFixture Build()
    {
        var fixture = new Fixture();
        fixture.Customize(new PortfolioGalleryItemCustomization());
        fixture.Customize(new SkillItemCustomization());
        fixture.Customize(new ServiceGalleryItemCustomization());
        fixture.Customize(new EducationCustomization());
        fixture.Customize(new WorkExperienceCustomization());
        fixture.Customize(new TalentCustomization());
        fixture.Register(() => DateOnly.FromDateTime(Faker.Date.Past(30)));
        fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList().ForEach(b => fixture.Behaviors.Remove(b));
        fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        return fixture;
    }

    public static ProfileEntity CreateProfileEntity(this IFixture fixture)
    {
        var entity = BuildProfileEntity(fixture)
            .Create();

        return entity;
    }

    public static IPostprocessComposer<ProfileEntity> BuildProfileEntity(this IFixture fixture)
    {
        return fixture.Build<ProfileEntity>()
            .With(x => x.Id, 0)
            .With(x => x.IsDeleted, false)
            .With(x => x.FirstName, Faker.Name.FirstName())
            .With(x => x.LastName, Faker.Name.LastName())
            .With(x => x.BirthDate, DateOnly.FromDateTime(Faker.Date.Past(30)))
            .With(x => x.ProfileImageUrl, Faker.Image.PicsumUrl())
            .With(x => x.MiddleName, TestUtilities.OptionalValue(Faker, f => f.Name.FirstName()))
            .With(x => x.SecondLastName, TestUtilities.OptionalValue(Faker, f => f.Name.LastName()))
            .Without(x => x.Resume)
            .Without(x => x.DeletedAt)
            .Without(x => x.DeletedBy)
            .Without(x => x.Talents)
            .Without(x => x.Portfolios)
            .Without(x => x.Services)
            .Without(x => x.Skills)
            .Without(x => x.Summaries)
            .Without(x => x.Testimonials);
    }

    public static ResumeEntity BuildResumeEntity(this IFixture fixture)
    {
        var entity = fixture.Build<ResumeEntity>()
            .With(x => x.Id, 0)
            .With(x => x.IsDeleted, false)
            .With(x => x.JobTitle, Faker.Name.JobTitle())
            .With(x => x.About, Faker.Lorem.Paragraph())
            .With(x => x.Summary, Faker.Lorem.Paragraph())
            .With(x => x.Overview, Faker.Lorem.Paragraph())
            .With(x => x.Title, TestUtilities.OptionalValue(Faker, f => f.Name.Prefix()))
            .With(x => x.Suffix, TestUtilities.OptionalValue(Faker, f => f.Name.Suffix()))
            .With(x => x.ProfileId, 0)
            .With(x => x.Profile, CreateProfileEntity(fixture))
            .Without(x => x.DeletedAt)
            .Without(x => x.DeletedBy)
            .Create();

        return entity;
    }

    public static TestimonialEntity BuildTestimonialEntity(this IFixture fixture)
    {
        var entity = fixture.Build<TestimonialEntity>()
            .With(x => x.Id, 0)
            .With(x => x.IsDeleted, false)
            .With(x => x.Name, Faker.Name.FullName())
            .With(x => x.JobTitle, Faker.Name.JobTitle())
            .With(x => x.Feedback, Faker.Lorem.Paragraph())
            .With(x => x.PhotoUrl, TestUtilities.OptionalValue(Faker, f => f.Image.PicsumUrl()))
            .With(x => x.Profile, CreateProfileEntity(fixture))
            .Without(x => x.DeletedAt)
            .Without(x => x.DeletedBy)
            .Create();

        return entity;
    }

    public static PortfolioEntity BuildPortfolioEntity(this IFixture fixture, int galleryItems = 5)
    {
        var portfolioGalleryItems = fixture.CreateMany<PortfolioGalleryItemEntity>(galleryItems).ToList();
        var portfolioEntity = fixture.Build<PortfolioEntity>()
            .With(x => x.Id, 0)
            .With(x => x.IsDeleted, false)
            .Without(x => x.DeletedAt)
            .Without(x => x.DeletedBy)
            .With(x => x.Profile, CreateProfileEntity(fixture))
            .With(x => x.GalleryItems, portfolioGalleryItems)
            .Create();

        return portfolioEntity;
    }

    public static SkillEntity BuildSkillEntity(this IFixture fixture, int skillItems = 5)
    {
        var skillDetailItems = fixture.CreateMany<SkillItemEntity>(skillItems).ToList();
        var skillEntity = fixture.Build<SkillEntity>()
            .With(x => x.Id, 0)
            .With(x => x.IsDeleted, false)
            .Without(x => x.DeletedAt)
            .Without(x => x.DeletedBy)
            .With(x => x.Profile, CreateProfileEntity(fixture))
            .With(x => x.SkillDetails, skillDetailItems)
            .Create();

        return skillEntity;
    }

    public static ServiceEntity BuildServiceEntity(this IFixture fixture, int serviceItems = 5)
    {
        var serviceGalleryItems = fixture.CreateMany<ServiceGalleryItemEntity>(serviceItems).ToList();
        var serviceEntity = fixture.Build<ServiceEntity>()
            .With(x => x.Id, 0)
            .With(x => x.Overview, Faker.Lorem.Paragraph())
            .With(x => x.IsDeleted, false)
            .Without(x => x.DeletedAt)
            .Without(x => x.DeletedBy)
            .With(x => x.Profile, CreateProfileEntity(fixture))
            .With(x => x.GalleryItems, serviceGalleryItems)
            .Create();

        return serviceEntity;
    }

    public static SummaryEntity BuildSummaryEntity(this IFixture fixture)
    {
        var summaryEntity = fixture.Build<SummaryEntity>()
            .With(x => x.Id, 0)
            .With(x => x.Introduction, Faker.Lorem.Paragraph())
            .With(x => x.IsDeleted, false)
            .Without(x => x.Education)
            .Without(x => x.WorkExperiences)
            .Without(x => x.DeletedAt)
            .With(x => x.Profile, CreateProfileEntity(fixture))
            .Without(x => x.DeletedBy)
            .Create();

        return summaryEntity;
    }

    public static SummaryEntity BuildSummaryEntityWithRelations(this IFixture fixture, int relatedItems = 5)
    {
        var educationList = fixture.CreateMany<EducationEntity>(relatedItems).ToList();
        var workExperienceList = fixture.CreateMany<WorkExperienceEntity>(relatedItems).ToList();

        var summaryEntity = fixture.Build<SummaryEntity>()
            .With(x => x.Id, 0)
            .With(x => x.Introduction, Faker.Lorem.Paragraph())
            .With(x => x.IsDeleted, false)
            .With(x => x.Profile, CreateProfileEntity(fixture))
            .With(x => x.Education, educationList)
            .With(x => x.WorkExperiences, workExperienceList)
            .Without(x => x.DeletedAt)
            .Without(x => x.DeletedBy)
            .Create();

        return summaryEntity;
    }
}