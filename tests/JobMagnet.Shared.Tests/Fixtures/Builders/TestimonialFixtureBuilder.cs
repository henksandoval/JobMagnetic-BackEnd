using AutoFixture;
using AutoFixture.Dsl;
using JobMagnet.Infrastructure.Entities;
using JobMagnet.Shared.Tests.Utils;

namespace JobMagnet.Shared.Tests.Fixtures.Builders;

public static class TestimonialFixtureBuilder
{
    public static IPostprocessComposer<TestimonialEntity> GetTestimonialEntityBuilder(this IFixture fixture)
    {
        var entityBuilder = fixture.Build<TestimonialEntity>()
            .With(x => x.Id, 0)
            .With(x => x.IsDeleted, false)
            .With(x => x.Name, FixtureBuilder.Faker.Name.FullName())
            .With(x => x.JobTitle, FixtureBuilder.Faker.Name.JobTitle())
            .With(x => x.Feedback, FixtureBuilder.Faker.Lorem.Paragraph())
            .With(x => x.PhotoUrl, TestUtilities.OptionalValue(FixtureBuilder.Faker, f => f.Image.PicsumUrl()))
            .With(x => x.Profile, fixture.GetProfileEntityBuilder().Create())
            .Without(x => x.DeletedAt)
            .Without(x => x.DeletedBy);

        return entityBuilder;
    }
}