using AutoFixture;
using JobMagnet.Infrastructure.Entities;

namespace JobMagnet.Shared.Tests.Fixtures.Builders;

public static class ContactInfoFixtureBuilder
{
    public static List<ContactInfoEntity> CreateContactInfoEntity(this IFixture fixture)
    {
        var postprocessComposer = fixture
            .Build<ContactInfoEntity>()
            .Without(x => x.Id)
            .Without(x => x.ResumeId)
            .Without(x => x.ContactType)
            .With(x => x.ContactTypeId, FixtureBuilder.Faker.Random.Int(1, 5));

        var contactInfoList = postprocessComposer
            .CreateMany(5).ToList();

        return contactInfoList;
    }
}