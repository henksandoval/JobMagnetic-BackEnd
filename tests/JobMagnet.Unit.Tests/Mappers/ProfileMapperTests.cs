using AutoFixture;
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

    [Fact(Skip = "Not implemented yet")]
    public void ShouldMapperProfileEntityToProfileViewModelWithPersonalData()
    {
        var profileBuilder = _fixture.GetProfileEntityBuilder();
        var resumeBuilder = _fixture.GetResumeEntityBuilder();
        var resumeEntity = resumeBuilder.Create();

        var talentEntities = _fixture.CreateMany<TalentEntity>(3).ToList();
        var profileEntity = profileBuilder
            .With(x => x.Talents, talentEntities)
            .With(x => x.Resume, resumeEntity)
            .Create();

        resumeEntity.Profile = profileEntity;

        var expectedResult = GetExpectedResult(profileEntity);

        var result = ProfileMapper.ToModel(profileEntity);

        result.ShouldNotBeNull();
    }

    private static ProfileViewModel GetExpectedResult(ProfileEntity entity)
    {
        var profileViewModel = new ProfileViewModel();

        var personaData = new PersonalDataViewModel(
            $"{entity.FirstName} {entity.MiddleName} {entity.LastName} {entity.SecondLastName}",
            entity.Talents.Select(x => x.Description).ToArray(),
            entity.Resume.ContactInfo.Select(c => new SocialNetworksViewModel(c.ContactType.Name, c.Value)).ToArray()
        );

        profileViewModel = profileViewModel with { PersonalData = personaData };

        return profileViewModel;
    }
}