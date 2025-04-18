using JobMagnet.Extensions.Utils;
using JobMagnet.Infrastructure.Entities;
using JobMagnet.ViewModels.Profile;
using Mapster;

namespace JobMagnet.Mappers;

internal static class ProfileMapper
{
    static ProfileMapper()
    {
        TypeAdapterConfig<ProfileEntity, ProfileViewModel>
            .NewConfig()
            .Map(dest => dest.PersonalData, src => new PersonalDataViewModel(
                GetFullName(src),
                src.Talents.Select(t => t.Description).ToArray(),
                src.Resume.ContactInfo.Select(c => new SocialNetworksViewModel(c.ContactType.Name, c.Value)).ToArray()
            ))
            .Map(dest => dest.About, src => new AboutViewModel(
                src.ProfileImageUrl,
                src.Resume.About,
                src.Resume.JobTitle,
                src.Resume.Overview,
                src.BirthDate!.Value,
                GetContactValue(src, "Website"),
                GetContactValue(src, "Mobile Phone"),
                src.Resume.Address!,
                src.BirthDate.GetAge(),
                src.Resume.Title!,
                GetContactValue(src, "Email"),
                "",
                src.Resume.Summary
            ));
    }

    internal static ProfileViewModel ToModel(ProfileEntity entity)
    {
        return entity.Adapt<ProfileViewModel>();
    }

    private static string GetFullName(ProfileEntity entity) =>
        string.Join(" ", entity.FirstName, entity.MiddleName, entity.LastName, entity.SecondLastName);

    private static string GetContactValue(ProfileEntity entity, string contactTypeName) =>
        entity.Resume.ContactInfo
            .FirstOrDefault(c => c.ContactType.Name == contactTypeName)
            ?.Value ?? string.Empty;
}
