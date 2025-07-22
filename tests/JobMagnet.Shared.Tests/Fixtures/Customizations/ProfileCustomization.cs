using AutoFixture;
using Bogus;
using JobMagnet.Application.Contracts.Responses.Base;
using JobMagnet.Application.UseCases.CvParser.DTO.RawDTOs;
using JobMagnet.Domain.Aggregates.Profiles;
using JobMagnet.Domain.Aggregates.Profiles.ValueObjects;
using JobMagnet.Shared.Abstractions;
using JobMagnet.Shared.Tests.Abstractions;
using JobMagnet.Shared.Tests.Utils;

namespace JobMagnet.Shared.Tests.Fixtures.Customizations;

public class ProfileCustomization : ICustomization
{
    private static readonly Faker Faker = FixtureBuilder.Faker;
    private readonly IClock _clock = new DeterministicClock();
    private readonly IGuidGenerator _guidGenerator = new SequentialGuidGenerator();

    public void Customize(IFixture fixture)
    {
        fixture.Customize<ProfileBase>(composer =>
            composer
                .With(x => x.FirstName, Faker.Name.FirstName())
                .With(x => x.LastName, Faker.Name.LastName())
                .With(x => x.BirthDate, () => GenerateAdultBirthDate())
                .With(x => x.ProfileImageUrl, Faker.Image.PicsumUrl())
                .With(x => x.MiddleName, TestUtilities.OptionalValue(Faker, f => f.Name.FirstName()))
                .With(x => x.SecondLastName, TestUtilities.OptionalValue(Faker, f => f.Name.LastName()))
                .OmitAutoProperties()
        );

        fixture.Customize<Profile>(composer =>
            composer
                .FromFactory(() => CreateProfileInstance(fixture))
                .OmitAutoProperties()
        );

        fixture.Customize<ProfileRaw>(composer =>
            composer
                .With(x => x.FirstName, Faker.Name.FirstName())
                .With(x => x.LastName, Faker.Name.LastName())
                .With(x => x.BirthDate, GenerateAdultBirthDateString)
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

    private Profile CreateProfileInstance(IFixture fixture)
    {
        var name = fixture.Create<PersonName>();
        var birthDate = fixture.Create<BirthDate>();
        var profileImage = fixture.Create<ProfileImage>();

        return Profile.CreateInstance(_guidGenerator,_clock, name, birthDate, profileImage);
    }

    private static string GenerateAdultBirthDateString()
    {
        var birthDateTime = GenerateAdultBirthDate();
        return birthDateTime.ToShortDateString();
    }

    private static DateOnly GenerateAdultBirthDate()
    {
        var today = DateTime.UtcNow;
        var minBirthDate = today.AddYears(-BirthDate.MaximumAge);
        var maxBirthDate = today.AddYears(-BirthDate.MinimumAge);

        var birthDateTime = Faker.Date.Between(minBirthDate, maxBirthDate);
        return DateOnly.FromDateTime(birthDateTime);
    }
}