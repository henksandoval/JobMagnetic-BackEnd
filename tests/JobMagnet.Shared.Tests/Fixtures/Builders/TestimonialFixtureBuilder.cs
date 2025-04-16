using AutoFixture;
using JobMagnet.Infrastructure.Entities;
using JobMagnet.Shared.Tests.Utils;

namespace JobMagnet.Shared.Tests.Fixtures.Builders;

public static class TestimonialFixtureBuilder
{
    public static TestimonialEntity BuildTestimonialEntity(this IFixture fixture)
    {
        var entity = fixture.Build<TestimonialEntity>()
            .With(x => x.Id, 0)
            .With(x => x.IsDeleted, false)
            .With(x => x.Name, FixtureBuilder.Faker.Name.FullName())
            .With(x => x.JobTitle, FixtureBuilder.Faker.Name.JobTitle())
            .With(x => x.Feedback, FixtureBuilder.Faker.Lorem.Paragraph())
            .With(x => x.PhotoUrl, TestUtilities.OptionalValue(FixtureBuilder.Faker, f => f.Image.PicsumUrl()))
            .With(x => x.Profile, fixture.CreateProfileEntity)
            .Without(x => x.DeletedAt)
            .Without(x => x.DeletedBy)
            .Create();

        return entity;
    }
}