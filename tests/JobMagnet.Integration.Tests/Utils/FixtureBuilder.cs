using AutoFixture;
using Bogus;
using JobMagnet.Infrastructure.Entities;

namespace JobMagnet.Integration.Tests.Utils;

public static class FixtureBuilder
{
    private static readonly Faker Faker = new();

    public static IFixture Build()
    {
        var fixture = new Fixture();
        fixture.Register(() => DateOnly.FromDateTime(Faker.Date.Past(30)));
        fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList().ForEach(b => fixture.Behaviors.Remove(b));
        fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        return fixture;
    }

    public static ResumeEntity BuildResumeEntity(this IFixture fixture)
    {
        var entity = fixture.Build<ResumeEntity>()
            .With(x => x.Id, 0)
            .With(x => x.IsDeleted, false)
            .With(x => x.FirstName, Faker.Name.FirstName())
            .With(x => x.LastName, Faker.Name.LastName())
            .With(x => x.JobTitle, Faker.Name.JobTitle())
            .With(x => x.BirthDate, DateOnly.FromDateTime(Faker.Date.Past(30)))
            .With(x => x.About, Faker.Lorem.Paragraph())
            .With(x => x.Summary, Faker.Lorem.Paragraph())
            .With(x => x.Overview, Faker.Lorem.Paragraph())
            .With(x => x.ProfileImageUrl, Faker.Image.PicsumUrl())
            .With(x => x.Title, OptionalValue(Faker, f => f.Name.Prefix()))
            .With(x => x.Suffix, OptionalValue(Faker, f => f.Name.Suffix()))
            .With(x => x.MiddleName, OptionalValue(Faker, f => f.Name.FirstName()))
            .With(x => x.SecondLastName, OptionalValue(Faker, f => f.Name.LastName()))
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
            .With(x => x.PhotoUrl, OptionalValue(Faker, f => f.Image.PicsumUrl()))
            .Without(x => x.DeletedAt)
            .Without(x => x.DeletedBy)
            .Create();

        return entity;
    }

    private static T? OptionalValue<T>(Faker faker, Func<Faker, T> valueGenerator, int probabilityPercentage = 50)
    {
        var random = new Random();
        return random.Next(100) < probabilityPercentage ? valueGenerator(faker) : default(T);
    }
}