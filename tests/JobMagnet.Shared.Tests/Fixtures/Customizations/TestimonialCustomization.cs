using AutoFixture;
using JobMagnet.Infrastructure.Entities;
using JobMagnet.Shared.Tests.Utils;

namespace JobMagnet.Shared.Tests.Fixtures.Customizations;

public class TestimonialCustomization : ICustomization
{
    public void Customize(IFixture fixture)
    {
        fixture.Customize<TestimonialEntity>(composer =>
            composer
                .With(x => x.Id, 0)
                .With(x => x.IsDeleted, false)
                .Without(x => x.DeletedAt)
                .Without(x => x.DeletedBy)
                .With(x => x.Name, FixtureBuilder.Faker.Name.FullName())
                .With(x => x.JobTitle, FixtureBuilder.Faker.Name.JobTitle())
                .With(x => x.Feedback, FixtureBuilder.Faker.Lorem.Paragraph())
                .With(x => x.PhotoUrl, TestUtilities.OptionalValue(FixtureBuilder.Faker, f => f.Image.PicsumUrl()))
                .With(x => x.Profile, fixture.Create<ProfileEntity>())
                .OmitAutoProperties()
        );
    }
}