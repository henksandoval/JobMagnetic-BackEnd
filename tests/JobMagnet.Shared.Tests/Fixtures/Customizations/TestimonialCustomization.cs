using AutoFixture;
using JobMagnet.Application.UseCases.CvParser.DTO.RawDTOs;
using JobMagnet.Domain.Aggregates.Profiles.Entities;
using JobMagnet.Domain.Core.Entities;
using JobMagnet.Shared.Tests.Utils;

namespace JobMagnet.Shared.Tests.Fixtures.Customizations;

public class TestimonialCustomization : ICustomization
{
    public void Customize(IFixture fixture)
    {
        fixture.Customize<Testimonial>(composer =>
            composer
                .FromFactory(() => new Testimonial(
                    FixtureBuilder.Faker.Name.FullName(),
                    FixtureBuilder.Faker.Name.JobTitle(),
                    FixtureBuilder.Faker.Lorem.Paragraph(),
                    TestUtilities.OptionalValue(FixtureBuilder.Faker, f => f.Image.PicsumUrl())
                ))
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
}