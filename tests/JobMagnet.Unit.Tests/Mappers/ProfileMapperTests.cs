using JobMagnet.Infrastructure.Entities;
using JobMagnet.ViewModels.Profile;

namespace JobMagnet.Unit.Tests.Mappers;

public class ProfileMapperTests
{
    [Fact]
    public void ShouldMapperProfileEntityToProfileViewModelWithPersonalData()
    {

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