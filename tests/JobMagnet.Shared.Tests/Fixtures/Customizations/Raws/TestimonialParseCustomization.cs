using AutoFixture;
using JobMagnet.Application.UseCases.CvParser.RawDTOs;
using JobMagnet.Shared.Tests.Utils;

namespace JobMagnet.Shared.Tests.Fixtures.Customizations.Raws;

public class TestimonialParseCustomization : ICustomization
{
    public void Customize(IFixture fixture)
    {
        fixture.Customize<TestimonialRaw>(composer =>
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