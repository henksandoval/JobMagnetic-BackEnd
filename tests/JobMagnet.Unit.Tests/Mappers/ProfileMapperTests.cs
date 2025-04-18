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

    private static PersonalDataViewModel GetPersonalDataViewModel(ProfileEntity entity)
    {
        return new PersonalDataViewModel(
            $"{entity.FirstName} {entity.LastName}",
            entity.Talents.Select(x => x.Description).ToArray(),
            entity.Resume.ContactInfo.Select(c => new SocialNetworksViewModel(
                c.ContactType.Name,
                c.Value,
                c.ContactType.IconClass,
                c.ContactType.IconUrl
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
            entity.Resume.Title!,
            email,
            "",
            entity.Resume.Summary
        );

        string GetContactValue(string contactTypeName) =>
            entity.Resume.ContactInfo
                .FirstOrDefault(x => x.ContactType.Name == contactTypeName)
                ?.Value ?? string.Empty;
    }
}