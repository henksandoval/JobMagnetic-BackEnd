using AutoFixture;
using JobMagnet.Infrastructure.Entities;
using JobMagnet.Shared.Tests.Utils;

namespace JobMagnet.Shared.Tests.Fixtures.Builders;

public static class ResumeFixtureBuilder
{
    public static ResumeEntity BuildResumeEntity(this IFixture fixture)
    {
        var contactInfoList = fixture
            .Build<ContactInfoEntity>()
            .Without(x => x.Id)
            .Without(x => x.ResumeId)
            .Without(x => x.ContactType)
            .With(x => x.ContactTypeId, FixtureBuilder.Faker.Random.Int(1, 5))
            .CreateMany(5).ToList();

        var entity = fixture.Build<ResumeEntity>()
            .With(x => x.Id, 0)
            .With(x => x.IsDeleted, false)
            .With(x => x.JobTitle, FixtureBuilder.Faker.Name.JobTitle())
            .With(x => x.About, FixtureBuilder.Faker.Lorem.Paragraph())
            .With(x => x.Summary, FixtureBuilder.Faker.Lorem.Paragraph())
            .With(x => x.Overview, FixtureBuilder.Faker.Lorem.Paragraph())
            .With(x => x.Title, TestUtilities.OptionalValue(FixtureBuilder.Faker, f => f.Name.Prefix()))
            .With(x => x.Suffix, TestUtilities.OptionalValue(FixtureBuilder.Faker, f => f.Name.Suffix()))
            .With(x => x.ProfileId, 0)
            .With(x => x.Profile, fixture.CreateProfileEntity)
            .With(x => x.ContactInfo, contactInfoList)
            .Without(x => x.DeletedAt)
            .Without(x => x.DeletedBy)
            .Create();

        return entity;
    }
}