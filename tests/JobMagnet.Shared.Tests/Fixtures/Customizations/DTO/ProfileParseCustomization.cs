using AutoFixture;
using JobMagnet.Application.UseCases.CvParser.ParsingDTOs;
using JobMagnet.Shared.Tests.Utils;

namespace JobMagnet.Shared.Tests.Fixtures.Customizations.DTO;

public class ProfileParseCustomization : ICustomization
{
    public void Customize(IFixture fixture)
    {
        fixture.Customize<ProfileParseDto>(composer =>
            composer
                .With(x => x.FirstName, FixtureBuilder.Faker.Name.FirstName())
                .With(x => x.LastName, FixtureBuilder.Faker.Name.LastName())
                .With(x => x.BirthDate, DateOnly.FromDateTime(FixtureBuilder.Faker.Date.Past(30)))
                .With(x => x.ProfileImageUrl, FixtureBuilder.Faker.Image.PicsumUrl())
                .With(x => x.MiddleName, TestUtilities.OptionalValue(FixtureBuilder.Faker, f => f.Name.FirstName()))
                .With(x => x.SecondLastName, TestUtilities.OptionalValue(FixtureBuilder.Faker, f => f.Name.LastName()))
                .Without(x => x.Resume)
                .Without(x => x.Talents)
                .Without(x => x.PortfolioGallery)
                .Without(x => x.Summary)
                .Without(x => x.Services)
                .Without(x => x.Skill)
                .Without(x => x.Testimonials)
        );
    }
}