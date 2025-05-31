using AutoFixture;
using JobMagnet.Domain.Core.Entities;
using JobMagnet.Shared.Tests.Utils;

namespace JobMagnet.Shared.Tests.Fixtures.Customizations.Entities;

public class TestimonialEntityCustomization : ICustomization
{
    public void Customize(IFixture fixture)
    {
        fixture.Customize<TestimonialEntity>(composer =>
            composer
                .With(x => x.Id, 0)
                .With(x => x.IsDeleted, false)
                .Without(x => x.DeletedAt)
                .Without(x => x.DeletedBy)
                .Do(ApplyCommonProperties)
                .With(x => x.Profile, fixture.Create<ProfileEntity>())
                .OmitAutoProperties()
        );
    }

    private static void ApplyCommonProperties(dynamic item)
    {
        item.Name = FixtureBuilder.Faker.Name.FullName();
        item.JobTitle = FixtureBuilder.Faker.Name.JobTitle();
        item.Feedback = FixtureBuilder.Faker.Lorem.Paragraph();
        item.PhotoUrl = TestUtilities.OptionalValue(FixtureBuilder.Faker, f => f.Image.PicsumUrl());
    }
}