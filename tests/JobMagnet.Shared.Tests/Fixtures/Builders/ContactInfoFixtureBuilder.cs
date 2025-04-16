using AutoFixture;
using AutoFixture.Dsl;
using JobMagnet.Infrastructure.Entities;

namespace JobMagnet.Shared.Tests.Fixtures.Builders;

public static class ContactInfoFixtureBuilder
{
    public static IPostprocessComposer<ContactInfoEntity> GetContactInfoEntityBuilder(this IFixture fixture)
    {
        return fixture
            .Build<ContactInfoEntity>()
            .Without(x => x.Id)
            .Without(x => x.ResumeId)
            .Without(x => x.Resume)
            .Without(x => x.ContactType)
            .With(x => x.ContactTypeId, FixtureBuilder.Faker.Random.Int(1, 5));
    }

    public static IPostprocessComposer<ContactInfoEntity> WithContactTypeEntity(this IPostprocessComposer<ContactInfoEntity> fixture)
    {
        return fixture
            .With(x => x.ContactType, fixture.Create<ContactTypeEntity>());
    }
}