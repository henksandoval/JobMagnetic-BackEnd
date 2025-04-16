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

    [Fact]
    public void ShouldMapperProfileEntityToProfileViewModelWithPersonalData()
    {
        var buildProfileEntity = _fixture.GetProfileEntityBuilder();
        var resumeEntityComposer = _fixture.GetResumeEntityBuilder(_fixture.CreateContactInfoEntity());
        var resumeEntity = resumeEntityComposer.Create();

        var talentEntities = _fixture.CreateMany<TalentEntity>(3).ToList();
        var profileEntity = buildProfileEntity
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