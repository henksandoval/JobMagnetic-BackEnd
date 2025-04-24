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
        TypeAdapterConfig<PortfolioGalleryEntity, PortfolioViewModel>
            .NewConfig()
            .Map(dest => dest.Image, src => src.UrlImage)
            .Map(dest => dest.Link, src => src.UrlLink)
            .Map(dest => dest.Video, src => src.UrlVideo);

        TypeAdapterConfig<ServiceGalleryItemEntity, ServiceDetailsViewModel>
            .NewConfig()
            .Map(dest => dest.BackgroundUrl, src => src.UrlImage)
            .Map(dest => dest.Name, src => src.Title);

        TypeAdapterConfig<TestimonialEntity, TestimonialsViewModel>
            .NewConfig()
            .Map(dest => dest.Testimonial, src => src.Feedback);

        TypeAdapterConfig<ProfileEntity, AboutViewModel>
            .NewConfig()
            .Map(dest => dest.ImageUrl, src => src.ProfileImageUrl)
            .Map(dest => dest.Description, src => src.Resume.About)
            .Map(dest => dest.Text, src => src.Resume.JobTitle)
            .Map(dest => dest.Hobbies, src => src.Resume.Overview)
            .Map(dest => dest.Birthday, src => src.BirthDate)
            .Map(dest => dest.Website, src => GetContactValue(src, "Website"))
            .Map(dest => dest.PhoneNumber, src => GetContactValue(src, "Mobile Phone"))
            .Map(dest => dest.Email, src => GetContactValue(src, "Email"))
            .Map(dest => dest.City, src => src.Resume.Address)
            .Map(dest => dest.Age, src => src.BirthDate.GetAge())
            .Map(dest => dest.Degree, src => src.Resume.Title ?? string.Empty)
            .Map(dest => dest.WorkExperience, src => src.Resume.Summary)
            .Map(dest => dest.Freelance, src => string.Empty);

        TypeAdapterConfig<SkillEntity, SkillSetViewModel>
            .NewConfig()
            .Map(dest => dest.SkillDetails,
                src => src.SkillDetails.Select(d => d.Adapt<SkillDetailsViewModel>()).ToArray());

        TypeAdapterConfig<ServiceEntity, ServiceViewModel>
            .NewConfig()
            .Map(dest => dest.ServiceDetails,
                src => src.GalleryItems.Select(item => item.Adapt<ServiceDetailsViewModel>()).ToArray());

        TypeAdapterConfig<SummaryEntity, SummaryViewModel>
            .NewConfig()
            .Map(dest => dest.Education,
                src => new EducationViewModel(src.Education.Select(e => new AcademicBackgroundViewModel(
                        e.Degree,
                        e.StartDate.ToString("yyyy-MM-dd"),
                        e.InstitutionName,
                        e.Description
                    )).ToArray()
                ),
                src => src.Education != null && src.Education.Count > 0)
            .Map(dest => dest.WorkExperience,
                src => new WorkExperienceViewModel(src.WorkExperiences.Select(w => new PositionViewModel(
                        w.JobTitle,
                        w.StartDate.ToString("yyyy-MM-dd"),
                        w.CompanyLocation,
                        string.Join(",", w.Responsibilities),
                        string.Join(",", w.Responsibilities),
                        string.Join(",", w.Responsibilities),
                        w.Description))
                    .ToArray()
                ),
                src => src.WorkExperiences != null && src.WorkExperiences.Count > 0);

        TypeAdapterConfig<ProfileEntity, ProfileViewModel>
            .NewConfig()
            .Map(dest => dest.PersonalData,
                src => new PersonalDataViewModel(
                    GetFullName(src),
                    src.Talents.Select(t => t.Description).ToArray(),
                    src.Resume!.ContactInfo.Select(c => new SocialNetworksViewModel(
                            c.ContactType.Name,
                            c.Value,
                            c.ContactType.IconClass ?? string.Empty,
                            c.ContactType.IconUrl ?? string.Empty))
                        .ToArray()),
                src => src.Resume != null && src.Resume.ContactInfo.Any() && src.Talents.Any())
            .Map(dest => dest.About, src => src.Adapt<AboutViewModel>(),
                src => src.Resume != null && src.BirthDate != null)
            .Map(dest => dest.Testimonials,
                src => src.Testimonials.Select(t => t.Adapt<TestimonialsViewModel>()).ToArray(),
                src => src.Testimonials.Any())
            .Map(dest => dest.PortfolioGallery,
                src => src.PortfolioGallery.Select(p => p.Adapt<PortfolioViewModel>()).ToArray(),
                src => src.PortfolioGallery.Any())
            .Map(dest => dest.SkillSet, src => src.Skill.Adapt<SkillSetViewModel>(),
                src => src.Skill != null && src.Skill.SkillDetails.Count > 0)
            .Map(dest => dest.Service, src => src.Services.Adapt<ServiceViewModel>(),
                src => src.Services != null && src.Services.GalleryItems.Count > 0)
            .Map(dest => dest.Summary, src => src.Summary.Adapt<SummaryViewModel>(),
                src => src.Summary != null);
    }

    internal static ProfileViewModel ToModel(this ProfileEntity entity)
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