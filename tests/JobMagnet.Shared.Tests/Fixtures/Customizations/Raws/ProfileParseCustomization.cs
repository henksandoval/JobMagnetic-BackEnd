using AutoFixture;
using JobMagnet.Application.UseCases.CvParser.DTO.RawDTOs;
using JobMagnet.Shared.Tests.Utils;

namespace JobMagnet.Shared.Tests.Fixtures.Customizations.Raws;

public class ProfileParseCustomization : ICustomization
{
    public void Customize(IFixture fixture)
    {
        fixture.Customize<ProfileRaw>(composer =>
            composer
                .With(x => x.FirstName, FixtureBuilder.Faker.Name.FirstName())
                .With(x => x.LastName, FixtureBuilder.Faker.Name.LastName())
                .With(x => x.BirthDate, DateOnly.FromDateTime(FixtureBuilder.Faker.Date.Past(30)).ToShortDateString())
                .With(x => x.ProfileImageUrl, FixtureBuilder.Faker.Image.PicsumUrl())
                .With(x => x.MiddleName, TestUtilities.OptionalValue(FixtureBuilder.Faker, f => f.Name.FirstName()))
                .With(x => x.SecondLastName, TestUtilities.OptionalValue(FixtureBuilder.Faker, f => f.Name.LastName()))
                .Without(x => x.Resume)
                .Without(x => x.Summary)
                .Without(x => x.Services)
                .Without(x => x.Skill)
                .With(x => x.Talents, () => [])
                .With(x => x.PortfolioGallery, () => [])
                .With(x => x.Testimonials, () => [])
        );
    }
}