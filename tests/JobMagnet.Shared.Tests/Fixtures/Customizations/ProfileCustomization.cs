using AutoFixture;
using Bogus;
using JobMagnet.Application.UseCases.CvParser.DTO.RawDTOs;
using JobMagnet.Domain.Aggregates.Profiles;
using JobMagnet.Shared.Tests.Utils;

namespace JobMagnet.Shared.Tests.Fixtures.Customizations;

public class ProfileCustomization : ICustomization
{
    private static readonly Faker Faker = FixtureBuilder.Faker;

    public void Customize(IFixture fixture)
    {
        fixture.Customize<Profile>(composer =>
            composer
                .FromFactory(() => new Profile(
                    new ProfileId(),
                    Faker.Random.Guid(),
                    Faker.Name.FirstName(),
                    Faker.Name.LastName(),
                    Faker.Image.PicsumUrl(200, 200),
                    DateOnly.FromDateTime(Faker.Date.Past(30)),
                    TestUtilities.OptionalValue(Faker, f => f.Name.FirstName()),
                    TestUtilities.OptionalValue(Faker, f => f.Name.LastName()
                )))
                .OmitAutoProperties()
        );

        fixture.Customize<ProfileRaw>(composer =>
            composer
                .With(x => x.FirstName, Faker.Name.FirstName())
                .With(x => x.LastName, Faker.Name.LastName())
                .With(x => x.BirthDate, DateOnly.FromDateTime(Faker.Date.Past(30)).ToShortDateString())
                .With(x => x.ProfileImageUrl, Faker.Image.PicsumUrl())
                .With(x => x.MiddleName, TestUtilities.OptionalValue(Faker, f => f.Name.FirstName()))
                .With(x => x.SecondLastName, TestUtilities.OptionalValue(Faker, f => f.Name.LastName()))
                .With(x => x.Resume, (ResumeRaw?)null)
                .With(x => x.Summary, (SummaryRaw?)null)
                .With(x => x.Services, (ServiceRaw?)null)
                .With(x => x.SkillSet, (SkillSetRaw?)null)
                .With(x => x.Talents, () => [])
                .With(x => x.Project, () => [])
                .With(x => x.Testimonials, () => [])
                .OmitAutoProperties()
        );
    }
}