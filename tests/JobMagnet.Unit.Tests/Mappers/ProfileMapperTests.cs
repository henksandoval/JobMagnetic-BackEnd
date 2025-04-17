using AutoFixture;
using AutoFixture.Dsl;
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

    [Fact]
    public void ShouldMapperProfileEntityToProfileViewModelWithPersonalData()
    {
        var profileBuilder = new ProfileEntityBuilder(_fixture)
            .WithResume()
            .WithContactInfo()
            .WithTalents()
            .WithPortfolio()
            .WithSummaries()
            .WithServices()
            .WithSkills()
            .WithTestimonials();

        var profile = profileBuilder.Build();

        var expectedResult = GetExpectedResult(profile);

        var result = ProfileMapper.ToModel(profile);

        result.ShouldNotBeNull();
    }

    private static ProfileViewModel GetExpectedResult(ProfileEntity entity)
    {
        var profileViewModel = new ProfileViewModel();

        var name = $"{entity.FirstName} {entity.MiddleName} {entity.LastName} {entity.SecondLastName}";
        var professions = entity.Talents.Select(x => x.Description).ToArray();
        var socialNetworksViewModels = entity.Resume.ContactInfo.Select(c => new SocialNetworksViewModel(c.ContactType.Name, c.Value)).ToArray();
        var personalData = new PersonalDataViewModel(
            name,
            professions,
            socialNetworksViewModels
        );

        profileViewModel = profileViewModel with { PersonalData = personalData };

        return profileViewModel;
    }
}