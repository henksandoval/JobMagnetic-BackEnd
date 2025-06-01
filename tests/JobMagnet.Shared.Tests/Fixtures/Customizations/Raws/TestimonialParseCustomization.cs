using AutoFixture;
using JobMagnet.Application.UseCases.CvParser.DTO.RawDTOs;
using JobMagnet.Shared.Tests.Utils;

namespace JobMagnet.Shared.Tests.Fixtures.Customizations.Raws;

public class TestimonialParseCustomization : ICustomization
{
    public void Customize(IFixture fixture)
    {
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