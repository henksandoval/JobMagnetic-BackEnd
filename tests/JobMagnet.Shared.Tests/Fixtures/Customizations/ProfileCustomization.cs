using AutoFixture;
using JobMagnet.Application.UseCases.CvParser.DTO.RawDTOs;
using JobMagnet.Domain.Core.Entities;
using JobMagnet.Shared.Tests.Utils;

namespace JobMagnet.Shared.Tests.Fixtures.Customizations;

public class ProfileCustomization : ICustomization
{
    public void Customize(IFixture fixture)
    {
        fixture.Customize<ProfileEntity>(composer =>
            composer
                .With(x => x.Id, 0)
                .With(x => x.IsDeleted, false)
                .With(x => x.FirstName, FixtureBuilder.Faker.Name.FirstName())
                .With(x => x.LastName, FixtureBuilder.Faker.Name.LastName())
                .With(x => x.BirthDate, DateOnly.FromDateTime(FixtureBuilder.Faker.Date.Past(30)))
                .With(x => x.ProfileImageUrl, FixtureBuilder.Faker.Image.PicsumUrl())
                .With(x => x.MiddleName, TestUtilities.OptionalValue(FixtureBuilder.Faker, f => f.Name.FirstName()))
                .With(x => x.SecondLastName, TestUtilities.OptionalValue(FixtureBuilder.Faker, f => f.Name.LastName()))
                .Without(x => x.DeletedAt)
                .Without(x => x.DeletedBy)
                .Without(x => x.Resume)
                .Without(x => x.Talents)
                .Without(x => x.PortfolioGallery)
                .Without(x => x.Summary)
                .Without(x => x.Services)
                .Without(x => x.SkillSet)
                .Without(x => x.Testimonials)
                .Without(x => x.PublicProfileIdentifiers)
        );

        fixture.Customize<ProfileRaw>(composer =>
            composer
                .With(x => x.FirstName, "FixtureBuilder.Faker.Name.FirstName()")
                .With(x => x.LastName, "FixtureBuilder.Faker.Name.LastName()")
                .With(x => x.BirthDate, DateOnly.FromDateTime(FixtureBuilder.Faker.Date.Past(30)).ToShortDateString())
                .With(x => x.ProfileImageUrl, FixtureBuilder.Faker.Image.PicsumUrl())
                .With(x => x.MiddleName, TestUtilities.OptionalValue(FixtureBuilder.Faker, f => f.Name.FirstName()))
                .With(x => x.SecondLastName, TestUtilities.OptionalValue(FixtureBuilder.Faker, f => f.Name.LastName()))
                .With(x => x.Resume, (ResumeRaw?) null)
                .With(x => x.Summary, (SummaryRaw?) null)
                .With(x => x.Services, (ServiceRaw?) null)
                .With(x => x.SkillSet, (SkillSetRaw?) null)
                .With(x => x.Talents, () => [])
                .With(x => x.PortfolioGallery, () => [])
                .With(x => x.Testimonials, () => [])
        );
    }
}