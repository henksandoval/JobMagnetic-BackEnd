using AutoFixture;
using JobMagnet.Application.UseCases.CvParser.DTO.RawDTOs;
using JobMagnet.Domain.Core.Entities;
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
                .Do(ApplyCommonProperties)
                .Without(x => x.Profile)
                .OmitAutoProperties()
        );

        fixture.Customize<TestimonialRaw>(composer =>
            composer.FromFactory(() => new TestimonialRaw(
                FixtureBuilder.Faker.Name.FullName(),
                FixtureBuilder.Faker.Name.JobTitle(),
                FixtureBuilder.Faker.Lorem.Paragraph(),
                TestUtilities.OptionalValue(FixtureBuilder.Faker, f => f.Image.PicsumUrl())
            ))
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