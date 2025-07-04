using AutoFixture;
using Bogus;
using JobMagnet.Application.UseCases.CvParser.DTO.RawDTOs;
using JobMagnet.Domain.Aggregates.Profiles.Entities;
using JobMagnet.Domain.Aggregates.Profiles.ValueObjects;
using JobMagnet.Shared.Abstractions;
using JobMagnet.Shared.Tests.Abstractions;
using JobMagnet.Shared.Tests.Utils;

namespace JobMagnet.Shared.Tests.Fixtures.Customizations;

public class TestimonialCustomization : ICustomization
{
    private readonly IClock _clock = new DeterministicClock();
    private readonly IGuidGenerator _guidGenerator = new SequentialGuidGenerator();
    private readonly Faker Faker = FixtureBuilder.Faker;

    public void Customize(IFixture fixture)
    {
        fixture.Customize<Testimonial>(composer =>
            composer
                .FromFactory(() => Testimonial.CreateInstance(
                    _guidGenerator,
                    new ProfileId(),
                    Faker.Name.FullName(),
                    Faker.Name.JobTitle(),
                    Faker.Lorem.Paragraph(),
                    TestUtilities.OptionalValue(Faker, f => f.Image.PicsumUrl())))
                .OmitAutoProperties()
        );

        fixture.Customize<TestimonialRaw>(composer =>
            composer.FromFactory(() => new TestimonialRaw(
                Faker.Name.FullName(),
                Faker.Name.JobTitle(),
                Faker.Lorem.Paragraph(),
                TestUtilities.OptionalValue(Faker, f => f.Image.PicsumUrl())
            ))
        );
    }
}