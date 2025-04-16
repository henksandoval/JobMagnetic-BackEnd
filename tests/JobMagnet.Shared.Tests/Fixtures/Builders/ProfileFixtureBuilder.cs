using AutoFixture;
using AutoFixture.Dsl;
using JobMagnet.Infrastructure.Entities;
using JobMagnet.Shared.Tests.Utils;

namespace JobMagnet.Shared.Tests.Fixtures.Builders;

public static class ProfileFixtureBuilder
{
    public static IPostprocessComposer<ProfileEntity> GetProfileEntityComposer(this IFixture fixture)
    {
        return fixture.Build<ProfileEntity>()
            .With(x => x.Id, 0)
            .With(x => x.IsDeleted, false)
            .With(x => x.FirstName, FixtureBuilder.Faker.Name.FirstName())
            .With(x => x.LastName, FixtureBuilder.Faker.Name.LastName())
            .With(x => x.BirthDate, DateOnly.FromDateTime(FixtureBuilder.Faker.Date.Past(30)))
            .With(x => x.ProfileImageUrl, FixtureBuilder.Faker.Image.PicsumUrl())
            .With(x => x.MiddleName, TestUtilities.OptionalValue(FixtureBuilder.Faker, f => f.Name.FirstName()))
            .With(x => x.SecondLastName, TestUtilities.OptionalValue(FixtureBuilder.Faker, f => f.Name.LastName()))
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
}