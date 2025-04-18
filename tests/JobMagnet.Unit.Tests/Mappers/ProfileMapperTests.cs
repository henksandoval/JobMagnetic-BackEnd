using AutoFixture;
using JobMagnet.Extensions.Utils;
using JobMagnet.Infrastructure.Entities;
using JobMagnet.Mappers;
using JobMagnet.Shared.Tests.Fixtures;
using JobMagnet.Shared.Tests.Fixtures.Builders;
using JobMagnet.ViewModels.Profile;
using Shouldly;

namespace JobMagnet.Unit.Tests.Mappers;

public class ProfileMapperTests
{
    private readonly IFixture _fixture = FixtureBuilder.Build();

    [Fact(DisplayName = "Should map ProfileEntity to ProfileViewModel when PersonalData is defined")]
    public void ShouldMapperProfileEntityToProfileViewModelWithPersonalData()
    {
        var profileBuilder = new ProfileEntityBuilder(_fixture)
            .WithTalents()
            .WithResume()
            .WithContactInfo();

        var profile = profileBuilder.Build();
        profile.SecondLastName = string.Empty;
        profile.MiddleName = string.Empty;

        var profileExpected = new ProfileViewModel();

        profileExpected = profileExpected with { PersonalData = GetPersonalDataViewModel(profile) };

        var result = ProfileMapper.ToModel(profile);

        result.ShouldNotBeNull();
        result.ShouldBeOfType<ProfileViewModel>();

        result.PersonalData!.ShouldBeEquivalentTo(profileExpected.PersonalData);
    }

    [Fact(DisplayName = "Should map ProfileEntity to ProfileViewModel when About is defined")]
    public void ShouldMapperProfileEntityToProfileViewModelWithAbout()
    {
        var profileBuilder = new ProfileEntityBuilder(_fixture)
            .WithResume()
            .WithContactInfo();

        var profile = profileBuilder.Build();

        var profileExpected = new ProfileViewModel();

        profileExpected = profileExpected with { About = GetAboutViewModel(profile) };

        var result = ProfileMapper.ToModel(profile);

        result.ShouldNotBeNull();
        result.ShouldBeOfType<ProfileViewModel>();

        result.About!.ShouldBeEquivalentTo(profileExpected.About);
    }

    [Fact(DisplayName = "Should map ProfileEntity to ProfileViewModel when Testimonial is defined")]
    public void ShouldMapperProfileEntityToProfileViewModelWithTestimonials()
    {
        var profileBuilder = new ProfileEntityBuilder(_fixture)
            .WithTestimonials();

        var profile = profileBuilder.Build();

        var profileExpected = new ProfileViewModel();

        profileExpected = profileExpected with { Testimonials = GetTestimonialViewModel(profile) };

        var result = ProfileMapper.ToModel(profile);

        result.ShouldNotBeNull();
        result.ShouldBeOfType<ProfileViewModel>();

        result.Testimonials!.ShouldBeEquivalentTo(profileExpected.Testimonials);
    }

    private static PersonalDataViewModel GetPersonalDataViewModel(ProfileEntity entity)
    {
        return new PersonalDataViewModel(
            $"{entity.FirstName} {entity.LastName}",
            entity.Talents.Select(x => x.Description).ToArray(),
            entity.Resume.ContactInfo.Select(c => new SocialNetworksViewModel(
                c.ContactType.Name,
                c.Value,
                c.ContactType.IconClass ?? string.Empty,
                c.ContactType.IconUrl ?? string.Empty
            )).ToArray()
        );
    }

    private static AboutViewModel GetAboutViewModel(ProfileEntity entity)
    {
        var webSite = GetContactValue("Website");
        var email = GetContactValue("Email");
        var mobilePhone = GetContactValue("Mobile Phone");

        return new AboutViewModel(
            entity.ProfileImageUrl,
            entity.Resume.About,
            entity.Resume.JobTitle,
            entity.Resume.Overview,
            entity.BirthDate!.Value,
            webSite,
            mobilePhone,
            entity.Resume.Address!,
            entity.BirthDate.GetAge(),
            entity.Resume.Title ?? string.Empty,
            email,
            "",
            entity.Resume.Summary
        );

        string GetContactValue(string contactTypeName) =>
            entity.Resume.ContactInfo
                .FirstOrDefault(x => x.ContactType.Name == contactTypeName)
                ?.Value ?? string.Empty;
    }

    private static TestimonialsViewModel[]? GetTestimonialViewModel(ProfileEntity profile)
    {
        return profile.Testimonials.Select(t => new TestimonialsViewModel(
                t.Name,
                t.JobTitle,
                t.PhotoUrl,
                t.Feedback))
            .ToArray();
    }
}