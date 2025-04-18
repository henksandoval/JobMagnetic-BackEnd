using JobMagnet.Extensions.Utils;
using JobMagnet.Infrastructure.Entities;
using JobMagnet.ViewModels.Profile;

namespace JobMagnet.Mappers;

internal static class ProfileMapper
{
    internal static ProfileViewModel ToModel(ProfileEntity entity)
    {
        var fullName = string.Join(" ", entity.FirstName, entity.MiddleName, entity.LastName, entity.SecondLastName);
        var profession = entity.Talents.Select(x => x.Description).ToArray();
        var socialNetworks = entity.Resume.ContactInfo.Select(c => new SocialNetworksViewModel(c.ContactType.Name, c.Value)).ToArray();
        var personalData = new PersonalDataViewModel(fullName, profession, socialNetworks);

        var webSite = entity.Resume.ContactInfo.FirstOrDefault(x => x.ContactType.Name == "Website")?.Value!;
        var email = entity.Resume.ContactInfo.FirstOrDefault(x => x.ContactType.Name == "Email")?.Value!;
        var mobilePhone = entity.Resume.ContactInfo.FirstOrDefault(x => x.ContactType.Name == "Mobile Phone")?.Value!;

        var about = new AboutViewModel(entity.ProfileImageUrl,
            entity.Resume.About,
            entity.Resume.JobTitle,
            entity.Resume.Overview,
            entity.BirthDate!.Value,
            webSite,
            mobilePhone,
            "",
            entity.BirthDate.GetAge(),
            entity.Resume.Title!,
            email,
            "",
            entity.Resume.Summary
            );
        
        var testimonials = entity.Testimonials.Select(x => new TestimonialsViewModel(x.Name, x.JobTitle, x.PhotoUrl, x.Feedback)).ToArray();

        var profile = new ProfileViewModel(personalData, about, testimonials);

        return profile;
    }
}