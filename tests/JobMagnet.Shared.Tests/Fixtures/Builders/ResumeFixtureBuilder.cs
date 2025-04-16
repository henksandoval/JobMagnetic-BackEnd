using AutoFixture;
using AutoFixture.Dsl;
using JobMagnet.Infrastructure.Entities;
using JobMagnet.Shared.Tests.Utils;

namespace JobMagnet.Shared.Tests.Fixtures.Builders;

public static class ResumeFixtureBuilder
{
    public static IPostprocessComposer<ResumeEntity> GetResumeEntityBuilder(this IFixture fixture, List<ContactInfoEntity> contactInfoList)
    {
        return fixture.Build<ResumeEntity>()
            .With(x => x.Id, 0)
            .With(x => x.IsDeleted, false)
            .With(x => x.JobTitle, FixtureBuilder.Faker.Name.JobTitle())
            .With(x => x.About, FixtureBuilder.Faker.Lorem.Paragraph())
            .With(x => x.Summary, FixtureBuilder.Faker.Lorem.Paragraph())
            .With(x => x.Overview, FixtureBuilder.Faker.Lorem.Paragraph())
            .With(x => x.Title, TestUtilities.OptionalValue(FixtureBuilder.Faker, f => f.Name.Prefix()))
            .With(x => x.Suffix, TestUtilities.OptionalValue(FixtureBuilder.Faker, f => f.Name.Suffix()))
            .With(x => x.ProfileId, 0)
            .With(x => x.Profile, fixture.GetProfileEntityBuilder().Create())
            .With(x => x.ContactInfo, contactInfoList)
            .Without(x => x.DeletedAt)
            .Without(x => x.DeletedBy);
    }

    public static ResumeEntity CreateResumeEntity(this IFixture fixture)
    {
        var contactInfoList = fixture.CreateContactInfoEntity();

        var entity = GetResumeEntityBuilder(fixture, contactInfoList)
            .Create();

        return entity;
    }
}