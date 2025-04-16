using AutoFixture;
using JobMagnet.Infrastructure.Entities;

namespace JobMagnet.Shared.Tests.Fixtures.Builders;

public static class SummaryFixtureBuilder
{
    public static SummaryEntity BuildSummaryEntity(this IFixture fixture)
    {
        var summaryEntity = fixture.Build<SummaryEntity>()
            .With(x => x.Id, 0)
            .With(x => x.Introduction, FixtureBuilder.Faker.Lorem.Paragraph())
            .With(x => x.IsDeleted, false)
            .Without(x => x.Education)
            .Without(x => x.WorkExperiences)
            .Without(x => x.DeletedAt)
            .With(x => x.Profile, fixture.GetProfileEntityComposer().Create())
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
            .With(x => x.Introduction, FixtureBuilder.Faker.Lorem.Paragraph())
            .With(x => x.IsDeleted, false)
            .With(x => x.Profile, fixture.GetProfileEntityComposer().Create())
            .With(x => x.Education, educationList)
            .With(x => x.WorkExperiences, workExperienceList)
            .Without(x => x.DeletedAt)
            .Without(x => x.DeletedBy)
            .Create();

        return summaryEntity;
    }
}