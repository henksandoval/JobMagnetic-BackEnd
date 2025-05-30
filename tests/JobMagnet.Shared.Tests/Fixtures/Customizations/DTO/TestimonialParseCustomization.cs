using AutoFixture;
using JobMagnet.Application.UseCases.CvParser.ParsingDTOs;
using JobMagnet.Domain.Entities;
using JobMagnet.Shared.Tests.Utils;

namespace JobMagnet.Shared.Tests.Fixtures.Customizations.DTO;

public class TestimonialParseCustomization : ICustomization
{
    public void Customize(IFixture fixture)
    {
        fixture.Customize<TestimonialParseDto>(composer =>
            composer
                .Do(ApplyCommonProperties)
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