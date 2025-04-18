using JobMagnet.Extensions.Utils;
using JobMagnet.Infrastructure.Entities;
using JobMagnet.ViewModels.Profile;
using Mapster;
// ReSharper disable ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
namespace JobMagnet.Mappers;

internal static class ProfileMapper
{
    static ProfileMapper()
    {
        TypeAdapterConfig<ProfileEntity, ProfileViewModel>
            .NewConfig()
            .Map(dest => dest.PersonalData,
                src => new PersonalDataViewModel(
                    GetFullName(src),
                    src.Talents.Select(t => t.Description).ToArray(),
                    src.Resume.ContactInfo.Select(c => new SocialNetworksViewModel(
                            c.ContactType.Name,
                            c.Value,
                            c.ContactType.IconClass ?? string.Empty,
                            c.ContactType.IconUrl ?? string.Empty))
                        .ToArray()
                ),
                src => src.Resume != null && src.Resume.ContactInfo.Any() && src.Talents.Any()
            )
            .Map(dest => dest.About, src => new AboutViewModel(
                    src.ProfileImageUrl,
                    src.Resume.About,
                    src.Resume.JobTitle,
                    src.Resume.Overview,
                    src.BirthDate!.Value,
                    GetContactValue(src, "Website"),
                    GetContactValue(src, "Mobile Phone"),
                    src.Resume.Address ?? string.Empty,
                    src.BirthDate.GetAge(),
                    src.Resume.Title ?? string.Empty,
                    GetContactValue(src, "Email"),
                    "",
                    src.Resume.Summary
                ),
                src => src.Resume != null && src.BirthDate != null
            )
            .Map(dest => dest.Testimonials, src => src.Testimonials
                    .Select(t => new TestimonialsViewModel(
                        t.Name,
                        t.JobTitle,
                        t.PhotoUrl,
                        t.Feedback))
                    .ToArray(),
                src => src.Testimonials.Any()
            )
            .Map(dest => dest.Service,
                src => src.Services
                    .Select(s => new ServiceViewModel(
                        s.Overview,
                        s.GalleryItems.Select(g => new ServiceDetailsViewModel(
                                g.Title,
                                g.Description,
                                g.UrlImage))
                            .ToArray()))
                    .FirstOrDefault() 
            );
    }   
    
    internal static ProfileViewModel ToModel(ProfileEntity entity)
    {
        return entity.Adapt<ProfileViewModel>();
    }

    private static string GetFullName(ProfileEntity entity) =>
        string.Join(" ", new[] { entity.FirstName, entity.MiddleName, entity.LastName, entity.SecondLastName }
            .Where(x => !string.IsNullOrWhiteSpace(x)));

    private static string GetContactValue(ProfileEntity entity, string contactTypeName)
    {
        return entity.Resume.ContactInfo
            .FirstOrDefault(c => c.ContactType.Name == contactTypeName)
            ?.Value ?? string.Empty;
    }
}